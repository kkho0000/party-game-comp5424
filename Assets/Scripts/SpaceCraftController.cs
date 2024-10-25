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
    public float speed = 40.0f; // 飞船的移动速度
    public float angularSpeed_x = 20.0f; // 飞船的x轴旋转速度
    public float angularSpeed_y = 20.0f; // 飞船的y轴旋转速度
    public float reverseSpeed = 20.0f; // 飞船的倒车速度
    private Transform trans; // 飞船的Transform组件
    private Camera mainCamera; // 主摄像头
    private TeleportController teleportController; // 传送控制器
    private Rigidbody rb; // 飞船的Rigidbody组件
    private bool isInObservationMode = false; // 是否处于观察模式
    public bool isReversing = false; // 是否正在倒车

    public Sprite filledSprite; // 能量球满时的图片
    public Sprite emptySprite; // 能量球空时的图片
    public SpriteRenderer[] spriteRenderer = new SpriteRenderer[3]; // 能量球UI
    public GameObject[] emptySpritecontainer = new GameObject[3]; // 能量球UI容器
    private int currentEnergy = 0; // 当前能量数量
    private int maxEnergy = 3; // 最大能量数量

    public Sprite lockstatus; // 视角锁定状态的图片
    public Sprite unlockstatus; // 视角解锁状态的图片
    public SpriteRenderer lockSpriteRenderer; // 视角锁定的UI显示

    private UnityEngine.XR.InputDevice headset;

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
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;

        // 初始化能量槽UI
        if (emptySprite != null)
        {
            for (int i = 0; i < 3; i++)
            {
                if (emptySpritecontainer[i] != null)
                {
                    spriteRenderer[i] = emptySpritecontainer[i].GetComponent<SpriteRenderer>();
                    if (spriteRenderer[i] == null)
                    {
                        Debug.LogWarning("未找到SpriteRenderer组件: " + emptySpritecontainer[i].name);
                    }
                }
                else
                {
                    Debug.LogWarning("能量槽[" + i + "]为空.");
                }
            }
        }

        lockSpriteRenderer.sprite = lockstatus;
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
        HandleMovement();
        //HandleTeleportation();
        //HandleObservationMode();
        //HandleStopSpin();
        HandleReverse();
    }

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

    private void HandleReverse()
    {
        if ( isReversing ) // (Input.GetKey(KeyCode.B))
        {
            rb.velocity = -trans.forward * reverseSpeed;
        }
    }

    /*键盘版传送
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

        if (Input.GetKeyDown(KeyCode.Q) && teleportController.IsInTeleportMode())
        {
            teleportController.CancelTeleport();
        }

        teleportController.UpdateTeleportMarkerPosition();
        teleportController.MoveTowardsTeleportTarget();
    }
    */

    //手柄版传送
    public void startTele()
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
        Debug.Log("执行传送");
        teleportController.UpdateTeleportMarkerPosition();
        teleportController.MoveTowardsTeleportTarget();
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

    /*键盘版切换观察模式
    private void HandleObservationMode()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isInObservationMode = !isInObservationMode;
            lockSpriteRenderer.sprite = isInObservationMode ? unlockstatus : lockstatus;
        }
    }
    */

    //HandleObservationMode的手柄版本
    public void SwitchObservationMode()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isInObservationMode = !isInObservationMode;
            lockSpriteRenderer.sprite = isInObservationMode ? unlockstatus : lockstatus;
        }
    }

    //碰撞检测
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnergyOrb"))
        {
            CollectEnergyOrb();
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            reduceEnergyOrb();
        }

        Debug.Log("与碰撞的物体: " + collision.gameObject.name);
    }

    /*键盘版复位
    private void HandleStopSpin()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.angularVelocity = Vector3.zero;
        }
    }
    */

    //手柄版复位
    public void ResetSpaceship()
    {
        rb.angularVelocity = Vector3.zero;
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
            Debug.Log("能量收集: " + currentEnergy);
        }
        else
        {
            Debug.LogWarning("SpriteRenderer 或 filledSprite 未设置.");
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
