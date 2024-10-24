using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private CanvasGroup canvasGroup;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;

        // 获取 CanvasGroup 组件
        canvasGroup = FindObjectOfType<CanvasGroup>();
        canvasGroup.alpha = 0.6f; // 设置半透明

        // 开始播放视频
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // 视频播放结束后隐藏视频
        canvasGroup.alpha = 0f;
    }
}