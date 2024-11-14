using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JumptoPlay : MonoBehaviour
{
    public Slider progressBar; // ����������
    private PersistentObject persistentObject;

    private void Start()
    {
        persistentObject = FindObjectOfType<PersistentObject>();
    }
    // ���������Լ����³���
    public void LoadScene(string sceneName)
    {
        
        StartCoroutine(LoadSceneAsync(sceneName));
        
        
    }


    // �첽���س���
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; // ��ֹ�����Զ�����

        // ������δ�������ʱ
        while (!operation.isDone)
        {
            // ���½�����ֵ
            // ע�⣺operation.progress ��Χ�� 0 �� 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            // �����ȴﵽ 0.9������������
            if (operation.progress >= 0.9f)
            {
                // �ȴ�һ��ʱ����ټ����
                //yield return new WaitForSeconds(1f); // ���Զ���ȴ�ʱ��
                operation.allowSceneActivation = true; // �����
            }

            yield return null; // �ȴ���һ֡
        }
    }
}
