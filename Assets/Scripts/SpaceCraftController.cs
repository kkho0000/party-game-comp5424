using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class SpaceCraftController : MonoBehaviour
{
    public float speed = 40.0f;
    public float angularSpeed_x = 20.0f;
    public float angularSpeed_y = 20.0f;
    private Transform trans;
    private Camera mainCamera;
    private TeleportController teleportController;
    private Rigidbody rb;
    private bool isInObservationMode = false; // 用于切换观察模式

    //能量球
    public Image[] energySlots; // Assign the energy slot images in the Inspector
    public Sprite filledSprite; // Assign the filled sprite in the Inspector
    public Sprite emptySprite; // Assign the empty sprite in the Inspector

    private int currentEnergy = 0;
    private int maxEnergy = 3;

    private UnityEngine.XR.InputDevice headset;

    // 初始化
    private void Awake()
    {
        trans = GetComponent<Transform>();
        mainCamera = Camera.main;
        teleportController = GetComponent<TeleportController>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        teleportController.Initialize(trans);

        // 设置Rigidbody的属性以处理碰撞
        rb.isKinematic = false;  // 允许物理交互
        rb.useGravity = false;   // 禁用重力（根据需要）
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 使用连续碰撞检测
    }

    // 每帧更新一次
    void Update()
    {
        HandleMovement();
        HandleTeleportation();
        HandleObservationMode(); // 添加处理观察模式的逻辑
        HandleStopSpin();        // 处理停止自旋的逻辑

        var leftHandedControllers = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristics = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristics, leftHandedControllers);

        foreach (var device in leftHandedControllers)
        {
            Debug.Log(string.Format("Device name '{0}' has characteristics '{1}'", device.name, device.characteristics.ToString()));
        }
    }

    // 根据摄像头的前方方向处理飞船的移动
    private void HandleMovement()
    {
        if (!isInObservationMode) // 如果不是观察模式，飞船的方向跟随摄像头
        {
            Vector3 targetDirection = mainCamera.transform.forward;
            Quaternion targetRotationXY = Quaternion.LookRotation(targetDirection, Vector3.up);
            Quaternion zRotation = Quaternion.Euler(0, 0, 90);
            Quaternion finalRotation = targetRotationXY * zRotation;
            if (Math.Abs(targetDirection.x) >= 0.15 || Math.Abs(targetDirection.y) >= 0.15)
            {
                trans.rotation = Quaternion.RotateTowards(trans.rotation, finalRotation, angularSpeed_x * Time.deltaTime);
            }
            else
            {
                trans.rotation = trans.rotation;
            }
        }

        // 使用Rigidbody的速度进行移动
        rb.velocity = trans.forward * speed;
    }

    // 处理传送逻辑
    private void HandleTeleportation()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!teleportController.IsInTeleportMode())
            {
                teleportController.EnterTeleportMode();
            }
            else if (!teleportController.IsTeleporting())
            {
                teleportController.StartTeleport();
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && teleportController.IsInTeleportMode())
        {
            teleportController.CancelTeleport();
        }

        // 更新传送标记并向传送目标移动（如果适用）
        teleportController.UpdateTeleportMarkerPosition();
        teleportController.MoveTowardsTeleportTarget();
    }

    // 处理观察模式的切换
    private void HandleObservationMode()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isInObservationMode = !isInObservationMode; // 切换观察模式状态
        }
    }

    // 处理与场景物体的碰撞
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnergyOrb"))
        {
            CollectEnergyOrb();
            Destroy(collision.gameObject); // 销毁金币
            //score += 100; // 增加分数
            //UpdateScoreUI();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Destroy(collision.gameObject); // 销毁敌人
            //life -= 1; // 减少分数
            //UpdateLifeUI();
        }

        // 日志记录碰撞的物体名称，便于调试
        Debug.Log("与碰撞的物体: " + collision.gameObject.name);

        // 碰撞时允许飞船自旋，不重置angularVelocity
    }

    // 处理停止自旋的逻辑
    private void HandleStopSpin()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 当按下空格键时停止飞船的自旋
            rb.angularVelocity = Vector3.zero;
            // Debug.Log("飞船自旋已停止");
        }
    }

    private void CollectEnergyOrb()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy++;
            UpdateEnergyUI();
        }
    }

    private void UpdateEnergyUI()
    {
       
    }

}
