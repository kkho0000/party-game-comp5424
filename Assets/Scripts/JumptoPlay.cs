using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumptoPlay : MonoBehaviour
{
    public Slider progressBar; // 进度条引用
    private PersistentObject persistentObject;

    private void Start()
    {
        persistentObject = FindObjectOfType<PersistentObject>();
    }
    // 方法调用以加载新场景
    public void LoadScene(string sceneName)
    {
        
        StartCoroutine(LoadSceneAsync(sceneName));
        
        
    }


    // 异步加载场景
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // 防止场景自动激活

        // 当场景未加载完成时
        while (!operation.isDone)
        {
            // 更新进度条值
            // 注意：operation.progress 范围是 0 到 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            // 当进度达到 0.9，允许场景激活
            if (operation.progress >= 0.9f)
            {
                // 等待一段时间后再激活场景
                //yield return new WaitForSeconds(1f); // 可自定义等待时间
                operation.allowSceneActivation = true; // 激活场景
            }

            yield return null; // 等待下一帧
        }
    }
}
