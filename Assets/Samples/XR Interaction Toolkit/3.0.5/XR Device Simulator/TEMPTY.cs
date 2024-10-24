using UnityEngine;
using System.Collections.Generic;

public class SpawnPrefabs : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        //���������Ҫ��������Item Creator��X��������һ������������Ԥ���塣
        //ʹ�÷�����ֱ�ӹ�����һ����ΪItem Creator�Ŀ������ϣ�����X���׼��Ҫ�ķ��򼴿ɡ�
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        for (int i = 0; i < 5; i++) // ���Ը���ʵ������������ɵ�����
        {
            Vector3 direction = originalRotation * Vector3.right;
            Vector3 spawnPosition = originalPosition + direction * i * 80;//���ɼ��
            spawnPosition.y += Random.Range(-50, 51);//���ɷ�Χ
            spawnPosition.z += Random.Range(-50, 51);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }
}