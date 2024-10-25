using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer videoPlayer; // ��Ƶ������
    private CanvasGroup targetCanvasGroup; // Ŀ�� Canvas ��

    void Start()
    {
        // ��ȡ VideoPlayer ���
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;

        // ����ֱ�Ӵ� VideoPlayer �ĸ������л�ȡ CanvasGroup
        targetCanvasGroup = GetComponentInParent<CanvasGroup>();

        // ���û���ҵ����ٴγ���ͨ�����ƻ�ȡ
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
            targetCanvasGroup.alpha = 0.6f; // ���ð�͸��
            Debug.Log("Bound to Canvas: " + targetCanvasGroup.gameObject.name);
        }
        else
        {
            Debug.LogWarning("CanvasGroup with name 'Canvas Countdown' not found. Please ensure it is correctly added in the scene.");
        }

        // ��ʼ������Ƶ
        videoPlayer.Play();
        Debug.Log("Video playback started");
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (targetCanvasGroup != null)
        {
            // ����һ���� Canvas ����Ϊ���ɼ�
            Debug.Log("Video ended, hiding Canvas: " + targetCanvasGroup.gameObject.name);
            targetCanvasGroup.gameObject.SetActive(false);

            // ���������ݻ� Canvas ����
            // Debug.Log("Video ended, destroying Canvas: " + targetCanvasGroup.gameObject.name);
            // Destroy(targetCanvasGroup.gameObject);
        }
        else
        {
            Debug.LogWarning("Target CanvasGroup not found");
        }
    }
}
