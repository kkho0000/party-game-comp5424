using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // ��ʼ������������ֵ
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
            Destroy(other.gameObject); // ���ٽ��
            score += 100; // ���ӷ���
            UpdateScoreUI();
        }else if(other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject); // ���ٵ���
            life -= 1; // ���ٷ���
            UpdateLifeUI();
        }
    }
    void Update()
    {
        // ÿ֡���·���
        //UpdateScoreUI();
        //UpdateLifeUI();
    }

    private void UpdateScoreUI()
    {
        // ����UI��ʾ�������߼�
        scoreText.text = "Score: " + score.ToString();
    }
    private void UpdateLifeUI()
    {
        // ����UI��ʾ����ֵ���߼�
        lifeText.text = "Life: " + life.ToString();
    }
}
