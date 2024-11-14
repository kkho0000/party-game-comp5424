using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // 获取 GameManager 实例
    }

    private void Update()
    {
        // 按下 A 键启动主机
        if (gameManager.client1)
        {
            gameManager.StartHost();

        }
        // 按下 B 键启动客户端
        if (gameManager.client2)
        {
            gameManager.StartClient();
        }
    }
}