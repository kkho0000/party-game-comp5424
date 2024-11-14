using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    
    public float moveSpeed = 5f;

    private void Update()
    {
        if (!IsOwner) return; // 只让拥有者控制移动

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            Move(moveDirection);
        }
    }

    private void Move(Vector3 direction)
    {
        Vector3 move = direction * moveSpeed * Time.deltaTime;
        transform.position += move;

        // 这里可以调用一个RPC来同步位置
        UpdatePositionServerRpc(transform.position);
    }

    [ServerRpc]
    private void UpdatePositionServerRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
        UpdatePositionClientRpc(newPosition);
    }

    [ClientRpc]
    private void UpdatePositionClientRpc(Vector3 newPosition)
    {
        if (!IsOwner) // 确保不是拥有者
        {
            transform.position = newPosition;
        }
    }
}

