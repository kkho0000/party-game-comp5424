using UnityEngine;
using System.Collections.Generic;

public class SpawnPrefabs : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        //本代码的主要功能是在Item Creator的X轴向上以一定间隔随机生成预制体。
        //使用方法是直接挂载于一个名为Item Creator的空物体上，将其X轴对准想要的方向即可。
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        for (int i = 0; i < 5; i++) // 可以根据实际需求调整生成的数量
        {
            Vector3 direction = originalRotation * Vector3.right;
            Vector3 spawnPosition = originalPosition + direction * i * 80;//生成间隔
            spawnPosition.y += Random.Range(-50, 51);//生成范围
            spawnPosition.z += Random.Range(-50, 51);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}