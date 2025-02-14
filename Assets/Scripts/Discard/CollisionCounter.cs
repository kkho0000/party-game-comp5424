using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionCounter : MonoBehaviour
{
    public int collisionCount = 0;
    public float timer = 0.0f;
    private bool isCounting = false;
    private HashSet<Collider> collidedObjects = new HashSet<Collider>();//使用HashSet来存储碰撞的对象，避免重复计数
    public RankingManager rankingManager;
    private ulong localClientId; // 存储本地客户端ID
    public void SetLocalClientId(ulong clientId)
    {
        localClientId = clientId; // 设置本地客户端ID
    }

    public PlaneInfo GetPlaneInfo()
    {
        Debug.Log("这是得到飞机信息的哪一行GetPlaneInfo: " + gameObject.name + " " + collisionCount + " " + timer); // 添加这一行
        return new PlaneInfo(gameObject.name, collisionCount, timer, localClientId);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("collision") && !collidedObjects.Contains(other))
        {
            Debug.Log("Collision detected");
            Debug.Log("Collision count: " + collisionCount);
            Debug.Log("Collision time: " + timer);
            collisionCount++;

            if (!isCounting)
            {
                isCounting = true;
                timer = 0.0f;
            }

            collidedObjects.Add(other);

            if (rankingManager != null)
            {
                rankingManager.UpdateRankingAfterCollision();
            }
            
            

        }else if(other.CompareTag("destination") && !collidedObjects.Contains(other)){
            // 跳转到结算场景，修改SceneA为结算场景的路由
            SceneManager.LoadScene(2);
        }
    }

    void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime;
        }
    }

}