using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab; // Ԥ��������
    private GameObject player1; // �洢player1����
    private GameObject player2; // �洢player2����
    public bool client1;
    public bool client2;

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
        // Debug.Log("Server started");

        // �ڷ������ϴ���player1������������
        /*player1 = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player1.name = "zheshifeiji1"; // ������Ϸ���������
        player1.GetComponent<NetworkObject>().Spawn(); // ȷ�����������ȷ����*/

        // �����ͻ��������¼�
        
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    }

    private void HandleClientConnected(ulong clientId)
    {
        
        // ���ͻ�������ʱ��������Ϊ�ÿͻ��˴���һ����Ҷ���
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        player.name = "player" + clientId; ; // ������Ϸ���������
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // ȷ�����������ȷ���ɲ���ͻ��˹���

        PlaneInfo planeInfo = player.GetComponent<CollisionCounter>().GetPlaneInfo(); // ���������������
        planeInfo.clientId = clientId; // ����clientId

        RankingManager rankingManager = FindObjectOfType<RankingManager>();
        rankingManager.SetLocalClientId(clientId);
        //SetPlayerNameServerRpc(clientId, player.name);
    }
    /*[ServerRpc]
    private void SetPlayerNameServerRpc(ulong clientId, string playerName)
    {
        // �ڷ������������������
        var playerObject =
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[clientId];
        playerObject.name = playerName;
        // Ҳ����������֪ͨ�ͻ��˸�������
        SetPlayerNameClientRpc(playerName, clientId);
    }
    [ClientRpc]
    private void SetPlayerNameClientRpc(string playerName, ulong clientId)
    {
        // �ڿͻ����������������
        var playerObject =
        NetworkManager.Singleton.SpawnManager.SpawnedObjects[clientId];
        playerObject.name = playerName;
    }*/
    public GameObject GetPlayer1()
    {
        return player1; // ����player1����
    }

    public GameObject GetPlayer2()
    {
        return player2; // ����player2����
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