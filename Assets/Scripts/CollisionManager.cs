using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    private OrbManager _orbManager;
    private CheckpointManager _checkpointManager;
    void Awake()
    {
        _orbManager = GetComponent<OrbManager>();
        _checkpointManager = GetComponent<CheckpointManager>();
        Debug.Log("New");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnergyOrb"))
        {
            Debug.Log("EnergyOrb");
            _orbManager.CollectEnergyOrb();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            _orbManager.reduceEnergyOrb();
        }else if(collision.gameObject.CompareTag("destination")){
            // 跳转到结算场景，修改SceneA为结算场景的路由
            SceneManager.LoadScene(0);
        }

        // Debug.Log("与碰撞的物体: " + collision.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (false) // (other.CompareTag("collision"))
        {
            _checkpointManager.updateCheckpoint(Convert.ToInt32(other.gameObject.name));
            // Debug.Log("Collision detected");
        }
    }
}
