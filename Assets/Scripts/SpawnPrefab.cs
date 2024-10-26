using UnityEngine;
using System.Collections.Generic;

public class SpawnPrefabs : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public GameObject energyOrbPrefab;
    public int spawnGap_Obstacle=150;//�ϰ����ɼ��
    public int spawnGap_EnergyOrb = 70;//���������ɼ��
    public int spawnCount_Obstacle=7;//�ϰ���������
    public int spawnCount_EnergyOrb = 20;//��������������
    public int spawnRangevertical = 60;//�������ɷ�Χ
    public int spawnRangehorizontal = 10;//�������ɷ�Χ

    void Start()
    {
        //���������Ҫ��������Item Creator��X��������һ������������Ԥ���塣
        //ʹ�÷�����ֱ�ӹ�����һ����ΪItem Creator�Ŀ������ϣ�����X���׼��Ҫ�ķ��򼴿ɡ�
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;
        energyOrbPrefab = Resources.Load<GameObject>("Art/Prefab/EnergyOrb");
        obstaclePrefab = Resources.LoadAll<GameObject>("Art/Prefab/Obstacle");
        for (int i = 0; i < spawnCount_Obstacle; i++) // ���Ը���ʵ������������ɵ�����
        {
            Vector3 direction = originalRotation * Vector3.right;
            Vector3 spawnPosition = originalPosition + direction * i * spawnGap_Obstacle;//���ɼ��
            spawnPosition.x += Random.Range(-spawnRangevertical, spawnRangevertical + 1);//�������ɷ�Χ
            spawnPosition.z += Random.Range(-spawnRangevertical, spawnRangevertical + 1);
            spawnPosition.y += Random.Range(-spawnRangehorizontal, spawnRangehorizontal + 1);//�������ɷ�Χ
            Instantiate(obstaclePrefab[i% obstaclePrefab.Length], spawnPosition, Quaternion.identity);
            
        }
        for (int i = 0; i < spawnCount_EnergyOrb; i++) // ���Ը���ʵ������������ɵ�����
        {
            Vector3 direction = originalRotation * Vector3.right;
            Vector3 spawnPosition = originalPosition + direction * i * spawnGap_EnergyOrb;//���ɼ��
            spawnPosition.x += Random.Range(-spawnRangevertical, spawnRangevertical + 1);//���ɷ�Χ
            spawnPosition.z += Random.Range(-spawnRangevertical, spawnRangevertical + 1);
            spawnPosition.y += Random.Range(-spawnRangehorizontal, spawnRangehorizontal + 1);//�������ɷ�Χ
            Instantiate(energyOrbPrefab, spawnPosition, Quaternion.identity);

        }
    }
}