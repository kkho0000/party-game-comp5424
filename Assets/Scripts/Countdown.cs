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

        // ��ȡ CanvasGroup ���
        canvasGroup = FindObjectOfType<CanvasGroup>();
        canvasGroup.alpha = 0.6f; // ���ð�͸��

        // ��ʼ������Ƶ
        videoPlayer.Play();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        // ��Ƶ���Ž�����������Ƶ
        canvasGroup.alpha = 0f;
    }
}