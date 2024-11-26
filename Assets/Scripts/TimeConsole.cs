using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeConsole : MonoBehaviour
{
    private TMPro.TMP_Text _inputField;
    public float elapsedTime; // 计时器开始时间
    void Awake () {
        _inputField = GetComponentInChildren<TMPro.TMP_Text>();
    }

    void Start()
    {
        // 启动协程，延迟3秒后开始计时
        elapsedTime = -3;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        DisplayTime(elapsedTime);
    }

    void DisplayTime(float timeToDisplay)
    {
        if ( elapsedTime < 0 ) return;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        _inputField.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
