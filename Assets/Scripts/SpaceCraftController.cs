using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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
    public Sprite filledSprite; // Assign the filled sprite in the Inspector
    public Sprite emptySprite; // Assign the empty sprite in the Inspector
    public SpriteRenderer[] spriteRenderer = new SpriteRenderer[3];
    public GameObject[] emptySpritecontainer = new GameObject[3];
    private int currentEnergy = 0;
    private int maxEnergy = 3;

    //视角锁
    public Sprite lockstatus;
    public Sprite unlockstatus;
    public SpriteRenderer lockSpriteRenderer;

    private UnityEngine.XR.InputDevice headset;
    public InputDevice rightHandController;

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

        //初始化能量槽
        if (emptySprite != null)
        {
            for (int i = 0; i < 3; i++)
            {
                if (emptySpritecontainer[i] != null)
                {
                    spriteRenderer[i] = emptySpritecontainer[i].GetComponent<SpriteRenderer>();
                    if (spriteRenderer[i] == null)
                    {
                        Debug.LogWarning("SpriteRenderer component not found on object: " + emptySpritecontainer[i].name);
                    }
                }
                else
                {
                    Debug.LogWarning("emptySprite[" + i + "] is null.");
                }
            }
        }
        //视角状态锁
        lockSpriteRenderer.sprite = lockstatus;

    }

    // 每帧更新一次
    void Update()
    {
        HandleMovement();
        HandleTeleportation();
        HandleObservationMode(); // 添加处理观察模式的逻辑
        HandleStopSpin();        // 处理停止自旋的逻辑
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
            if (Math.Abs(targetDirection.x) >= 0.08 || Math.Abs(targetDirection.y) >= 0.08)
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

    // 处理传送逻辑，按两次A键传送，一次A一次B取消传送
    private void HandleTeleportation()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (!teleportController.IsInTeleportMode())
            {
                teleportController.EnterTeleportMode();
            }
            else if (!teleportController.IsTeleporting())
            {
                if (currentEnergy == 3)
                {
                    teleportController.StartTeleport();
                }
                else
                {
                    teleportController.CancelTeleport();
                }
                clearEnergyOrb();
                clearEnergyUI();
            }
        }

        if(Input.GetKeyDown(KeyCode.Q) && teleportController.IsInTeleportMode())
        {
            teleportController.CancelTeleport();
        }

        // 更新传送标记并向传送目标移动（如果适用）
        teleportController.UpdateTeleportMarkerPosition();
        teleportController.MoveTowardsTeleportTarget();
    }

    //为了配合按钮调用HandleTeleportation()函数
    public void TeleportSpaceship()
    {
        HandleTeleportation();
    }

    // 处理观察模式的切换,按下手柄上的X键切换（键盘O键）
    private void HandleObservationMode()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isInObservationMode = !isInObservationMode; // 切换观察模式状态
            lockSpriteRenderer.sprite = isInObservationMode ? unlockstatus : lockstatus; // 切换视角锁状态
        }
    }

    // 处理与场景物体的碰撞
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnergyOrb"))
        {
            CollectEnergyOrb();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject); // 销毁敌人
            reduceEnergyOrb();
        }

        // 日志记录碰撞的物体名称，便于调试
        Debug.Log("与碰撞的物体: " + collision.gameObject.name);

        // 碰撞时允许飞船自旋，不重置angularVelocity
    }

    // 处理停止自旋的逻辑，按下手柄Y键/键盘空格键停止自旋
    private void HandleStopSpin()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 当按下空格键时停止飞船的自旋
            rb.angularVelocity = Vector3.zero;
            // Debug.Log("飞船自旋已停止");
        }
    }
    //为了配合按钮调用StopSpin()函数
    public void ResetSpaceship()
    {
        HandleStopSpin();
        Debug.Log("飞船自旋已停止");
    }

    private void CollectEnergyOrb()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy++;
            UpdateEnergyUI(currentEnergy);
        }
    }

    private void UpdateEnergyUI(int currentEnergy)
    {
        if (spriteRenderer != null && filledSprite != null)
        {
            spriteRenderer[currentEnergy - 1].sprite = filledSprite;
            Debug.Log("Energy collected: " + currentEnergy);
        }
        else
        {
            Debug.LogWarning("SpriteRenderer or filledSprite is not set.");
        }
    }

    private void clearEnergyOrb()
    {
        currentEnergy = 0;
    }

    private void clearEnergyUI()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer[i].sprite = emptySprite;
        }
        currentEnergy = 0;
    }

    private void reduceEnergyOrb()
    {
        if (currentEnergy == 0)
        {
            return;
        }
        else
        {
            currentEnergy--;
            spriteRenderer[currentEnergy].sprite = emptySprite;
        }
    }
}

