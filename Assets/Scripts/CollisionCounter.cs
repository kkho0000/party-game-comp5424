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

    public PlaneInfo GetPlaneInfo()
    {
        Debug.Log("这是得到飞机信息的哪一行GetPlaneInfo: " + gameObject.name + " " + collisionCount + " " + timer); // 添加这一行
        return new PlaneInfo(gameObject.name, collisionCount, timer);
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
            
            if (collisionCount >= 51)
            {
                // 跳转到结算场景，修改SceneA为结算场景的路由
                // SceneManager.LoadScene("SceneA"); 
            }

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


