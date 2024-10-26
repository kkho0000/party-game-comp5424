using UnityEngine;
using System.Collections.Generic;

public class SpawnPrefabs : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public GameObject energyOrbPrefab;
    public int spawnGap_Obstacle=150;//障碍生成间隔
    public int spawnGap_EnergyOrb = 70;//能量球生成间隔
    public int spawnCount_Obstacle=7;//障碍生成数量
    public int spawnCount_EnergyOrb = 20;//能量球生成数量
    public int spawnRangevertical = 60;//横向生成范围
    public int spawnRangehorizontal = 10;//纵向生成范围

    void Start()
    {
        //本代码的主要功能是在Item Creator的X轴向上以一定间隔随机生成预制体。
        //使用方法是直接挂载于一个名为Item Creator的空物体上，将其X轴对准想要的方向即可。
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        energyOrbPrefab = Resources.Load<GameObject>("Art/Prefab/EnergyOrb");
        obstaclePrefab = Resources.LoadAll<GameObject>("Art/Prefab/Obstacle");
        for (int i = 0; i < spawnCount_Obstacle; i++) // 可以根据实际需求调整生成的数量
        {
            Vector3 direction = originalRotation * Vector3.right;
            Vector3 spawnPosition = originalPosition + direction * i * spawnGap_Obstacle;//生成间隔
            spawnPosition.x += Random.Range(-spawnRangevertical, spawnRangevertical + 1);//横向生成范围
            spawnPosition.z += Random.Range(-spawnRangevertical, spawnRangevertical + 1);
            spawnPosition.y += Random.Range(-spawnRangehorizontal, spawnRangehorizontal + 1);//纵向生成范围
            Instantiate(obstaclePrefab[i% obstaclePrefab.Length], spawnPosition, Quaternion.identity);
            
        }
        for (int i = 0; i < spawnCount_EnergyOrb; i++) // 可以根据实际需求调整生成的数量
        {
            Vector3 direction = originalRotation * Vector3.right;
            Vector3 spawnPosition = originalPosition + direction * i * spawnGap_EnergyOrb;//生成间隔
            spawnPosition.x += Random.Range(-spawnRangevertical, spawnRangevertical + 1);//生成范围
            spawnPosition.z += Random.Range(-spawnRangevertical, spawnRangevertical + 1);
            spawnPosition.y += Random.Range(-spawnRangehorizontal, spawnRangehorizontal + 1);//纵向生成范围
            Instantiate(energyOrbPrefab, spawnPosition, Quaternion.identity);

        }
    }
}