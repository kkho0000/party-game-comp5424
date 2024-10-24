using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplay : MonoBehaviour
{
    public Rigidbody spaceshipRigidbody;  // 飞船的 Rigidbody 组件
    public TMP_Text speedText;  // 用于显示速度的 UI Text 元素

    public TMP_Text timerText; // UI Text 用于显示计时信息
    private float elapsedTime = 0f; // 已经过时间
    private bool timerRunning = false; // 计时器是否正在运行
    void Start()
    {
        StartTimer();
    }
    void Update()
    {
        UpdateSpeed();  // 更新飞船速度

        if (timerRunning)
    {
        UpdateTimer(Time.deltaTime);
    }
    }
     
    void UpdateSpeed()
    {
        float speed = spaceshipRigidbody.velocity.magnitude;  // 获取飞船当前速度的大小
        speedText.text = speed.ToString("F1") + " m/s";  // 更新 UI Text 显示当前速度
        Debug.Log(speed);
    }
    void StartTimer()
    {
        timerRunning = true;
    }

    void UpdateTimer(float deltaTime)
    {
        elapsedTime += deltaTime; // 累加已经过时间
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // 格式化时间显示为 MM:SS
    }
}
