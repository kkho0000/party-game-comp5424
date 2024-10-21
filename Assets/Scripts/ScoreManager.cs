using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // 初始化分数和生命值
    public TMP_Text scoreText;
    public TMP_Text lifeText;
    private int score = 0;
    private int life = 5;

    void Start()
    {
        scoreText.text = "Score: " + 0;
        lifeText.text = "Life: " + 5;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject); // 销毁金币
            score += 100; // 增加分数
            UpdateScoreUI();
        }else if(other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // 销毁敌人
            life -= 1; // 减少分数
            UpdateLifeUI();
        }
    }
    void Update()
    {
        // 每帧更新分数
        //UpdateScoreUI();
        //UpdateLifeUI();
    }

    private void UpdateScoreUI()
    {
        // 更新UI显示分数的逻辑
        scoreText.text = "Score: " + score.ToString();
    }
    private void UpdateLifeUI()
    {
        // 更新UI显示生命值的逻辑
        lifeText.text = "Life: " + life.ToString();
    }
}
