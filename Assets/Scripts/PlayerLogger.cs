using UnityEngine;

public class PlayerLogger : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // 查找场景中的GameManager
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            *//*if (gameManager != null && gameManager.GetPlayer1() != null)
            {
                Debug.Log("Player 1 Name: " + gameManager.GetPlayer1().name); // 输出player1的名字
            }*//*
            else
            {
                Debug.Log("Player 1 does not exist.");
            }
        */
    }
}