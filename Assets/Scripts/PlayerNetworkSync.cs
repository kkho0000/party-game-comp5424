using UnityEngine;
using Unity.Netcode;

public class PlayerNetworkSync : NetworkBehaviour
{
    private SpaceCraftController playerMovement; // 引用玩家移动脚本

    private void Start()
    {
        playerMovement = GetComponent<SpaceCraftController>(); // 获取玩家移动脚本的引用
    }

    private void Update()
    {
        if (IsOwner)
        {
            // 只有拥有者更新位置
            UpdatePositionServerRpc(transform.position);
        }
    }

    [ServerRpc]
    private void UpdatePositionServerRpc(Vector3 newPosition)
    {
        // 更新服务器上的位置
        transform.position = newPosition;
        UpdatePositionClientRpc(newPosition); // 同步到所有客户端
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newPosition)
    {
        if (!IsOwner)
        {
            // 只有非拥有者更新位置
            transform.position = newPosition;
        }
    }
}