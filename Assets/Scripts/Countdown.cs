using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer; // 视频播放器
    private CanvasGroup targetCanvasGroup; // 目标 Canvas 组

    void Start()
    {
        // 获取 VideoPlayer 组件
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;

        // 尝试直接从 VideoPlayer 的父对象中获取 CanvasGroup
        targetCanvasGroup = GetComponentInParent<CanvasGroup>();

        // 如果没有找到，再次尝试通过名称获取
        if (targetCanvasGroup == null)
        {
            GameObject countdownCanvas = GameObject.Find("Canvas Countdown");
            if (countdownCanvas != null)
            {
                targetCanvasGroup = countdownCanvas.GetComponent<CanvasGroup>();
            }
        }

        if (targetCanvasGroup != null)
        {
            targetCanvasGroup.alpha = 0.6f; // 设置半透明
            Debug.Log("Bound to Canvas: " + targetCanvasGroup.gameObject.name);
        }
        else
        {
            Debug.LogWarning("CanvasGroup with name 'Canvas Countdown' not found. Please ensure it is correctly added in the scene.");
        }

        // 开始播放视频
        videoPlayer.Play();
        Debug.Log("Video playback started");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (targetCanvasGroup != null)
        {
            // 方法一：将 Canvas 设置为不可见
            Debug.Log("Video ended, hiding Canvas: " + targetCanvasGroup.gameObject.name);
            targetCanvasGroup.gameObject.SetActive(false);

            // 方法二：摧毁 Canvas 对象
            // Debug.Log("Video ended, destroying Canvas: " + targetCanvasGroup.gameObject.name);
            // Destroy(targetCanvasGroup.gameObject);
        }
        else
        {
            Debug.LogWarning("Target CanvasGroup not found");
        }
    }
}
