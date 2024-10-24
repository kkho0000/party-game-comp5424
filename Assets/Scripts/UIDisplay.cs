using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    public Rigidbody spaceshipRigidbody;  // �ɴ��� Rigidbody ���
    public TMP_Text speedText;  // ������ʾ�ٶȵ� UI Text Ԫ��

    public TMP_Text timerText; // UI Text ������ʾ��ʱ��Ϣ
    private float elapsedTime = 0f; // �Ѿ���ʱ��
    private bool timerRunning = false; // ��ʱ���Ƿ���������
    void Start()
    {
        StartTimer();
    }
    void Update()
    {
        UpdateSpeed();  // ���·ɴ��ٶ�

        if (timerRunning)
    {
        UpdateTimer(Time.deltaTime);
    }
    }
     
    void UpdateSpeed()
    {
        float speed = spaceshipRigidbody.velocity.magnitude;  // ��ȡ�ɴ���ǰ�ٶȵĴ�С
        speedText.text = speed.ToString("F1") + " m/s";  // ���� UI Text ��ʾ��ǰ�ٶ�
        Debug.Log(speed);
    }
    void StartTimer()
    {
        timerRunning = true;
    }

    void UpdateTimer(float deltaTime)
    {
        elapsedTime += deltaTime; // �ۼ��Ѿ���ʱ��
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // ��ʽ��ʱ����ʾΪ MM:SS
    }
}
