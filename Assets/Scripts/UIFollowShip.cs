using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.Receiver.Primitives;

public class UIFollowShip : MonoBehaviour
{
    public Transform spaceshipTransform; // Assign in the Inspector
    private Vector3 uiOffset;
    private Quaternion uiRoOffset;
    public GameObject prefab;

    void Start()
    {
        for (int i = 0; i < 5; i++) // 可以根据实际需求调整生成的数量
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + i * 200, transform.position.y + Random.Range(-50, 51), transform.position.z + Random.Range(-50, 51));
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}
