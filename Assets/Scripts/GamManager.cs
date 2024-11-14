using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab; // 预制体引用
    private GameObject player1; // 存储player1对象
    private GameObject player2; // 存储player2对象
    public bool client1;
    public bool client2;

    private void Start()
    {
        // 注册事件
        
    }

    private void Update()
    {
        // 这里可以保留原有的按键检测逻辑，或者移除
    }

    public void StartHost()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    private void HandleServerStarted()
    {
        Debug.Log("Server started");

        // 在服务器上创建player1对象并设置名称
        /*player1 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player1.name = "zheshifeiji1"; // 设置游戏对象的名字
        player1.GetComponent<NetworkObject>().Spawn(); // 确保网络对象被正确生成*/

        // 监听客户端连接事件
        
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        
        // 当客户端连接时，服务器为该客户端创建一个玩家对象
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.name = "player" + clientId; ; // 设置游戏对象的名字
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // 确保网络对象被正确生成并与客户端关联

        PlaneInfo planeInfo = player.GetComponent<CollisionCounter>().GetPlaneInfo(); // 假设你有这个方法
        planeInfo.clientId = clientId; // 设置clientId

        RankingManager rankingManager = FindObjectOfType<RankingManager>();
        rankingManager.SetLocalClientId(clientId);
        //SetPlayerNameServerRpc(clientId, player.name);
    }
    /*[ServerRpc]
    private void SetPlayerNameServerRpc(ulong clientId, string playerName)
    {
        // 在服务器上设置玩家名称
        var playerObject =
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[clientId];
        playerObject.name = playerName;
        // 也可以在这里通知客户端更新名称
        SetPlayerNameClientRpc(playerName, clientId);
    }
    [ClientRpc]
    private void SetPlayerNameClientRpc(string playerName, ulong clientId)
    {
        // 在客户端上设置玩家名称
        var playerObject =
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[clientId];
        playerObject.name = playerName;
    }*/
    public GameObject GetPlayer1()
    {
        return player1; // 返回player1对象
    }

    public GameObject GetPlayer2()
    {
        return player2; // 返回player2对象
    }
    public void OnPlayer1()
    {
        //NetworkManager.Singleton.StartHost();
        client1= true;
    }
    public void OnPlayer2()
    {
        client2= true;
    }
}