using System;
using System.Collections;
using UnityEngine;

public class SpaceCraftController : MonoBehaviour
{
    public float speed = 40.0f; // 飞船的移动速度
    public float angularSpeed = 20.0f; // 飞船的x轴旋转速度
    public float reverseSpeed = 20.0f; // 飞船的倒车速度

    private bool isInObservationMode = false; // 是否处于观察模式
    public bool isReversing = false; // 是否正在倒车

    private Transform trans; // 飞船的Transform组件
    private Camera mainCamera; // 主摄像头
    private Rigidbody rb; // 飞船的Rigidbody组件
    
    private TeleportController teleportController; // 传送控制器
    private OrbManager _orbManager; // 能量球管理器
    private LockConsole _lockConsole; // 锁定控制台

    private void Awake()
    {
        trans = GetComponent<Transform>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        teleportController = GetComponent<TeleportController>();
        _orbManager = GetComponent<OrbManager>();
        _lockConsole = GetComponentInChildren<LockConsole>();
    }

    private void Start()
    {
        teleportController.Initialize(trans);
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
        StartCoroutine(DelayStart());
    }

    private IEnumerator DelayStart()
    {
        enabled = false;
        yield return new WaitForSeconds(3.5f);
        enabled = true;
    }

    void Update()
    {
        if (!isInObservationMode) // 如果不是观察模式，飞船的方向跟随摄像头
        {
            if (!teleportController.IsTeleporting())
            {
                Vector3 targetDirection = mainCamera.transform.forward;
                Quaternion targetRotationXY = Quaternion.LookRotation(targetDirection, Vector3.up);
                Quaternion zRotation = Quaternion.Euler(0, 0, 90);
                Quaternion finalRotation = targetRotationXY * zRotation;
                if (Math.Abs(targetDirection.x) >= 0.08 || Math.Abs(targetDirection.y) >= 0.08)
                {
                    trans.rotation = Quaternion.RotateTowards(trans.rotation, finalRotation, angularSpeed * Time.deltaTime);
                }
                else
                {
                    trans.rotation = trans.rotation;
                }

                // 使用Rigidbody的速度进行移动、
                rb.velocity = trans.forward * speed;

                if (teleportController.IsInTeleportMode())
                {
                    teleportController.UpdateTeleportMarkerPosition();
                }
            }
            else
            {
                teleportController.MoveTowardsTeleportTarget();
            }
        }                  
    }

    //手柄版传送
    public void startTele()
    {
        if (!teleportController.IsInTeleportMode())
        {
            teleportController.EnterTeleportMode();
        }
        else if (!teleportController.IsTeleporting())
        {
            if (_orbManager.GetCurrentEnergy() == 3)
            {
                _orbManager.clearEnergyOrb();
                teleportController.StartTeleport();
                Debug.Log("执行传送");
            }
            else
            {
                teleportController.CancelTeleport();
            }
            
        }
        
    }

    //手柄版取消传送
    public void cancelTele()
    {
        if (teleportController.IsInTeleportMode())
        {
            Debug.Log("取消传送");
            teleportController.CancelTeleport();
        }
    }

    //HandleObservationMode的手柄版本
    public void SwitchObservationMode()
    {
        isInObservationMode = !isInObservationMode;
        _lockConsole.SetLocked(!isInObservationMode);
    }

    //手柄版复位
    public void ResetSpaceship()
    {
        rb.angularVelocity = Vector3.zero;
        Debug.Log("飞船自旋已停止");
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

}
