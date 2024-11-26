using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

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
    private SpeedConsole _speedConsole; // speed console
    private Vector3 lastPosition;
    private float realSpeed;

    private XRJoystick _joystick; // 摇杆控制器
    private float yawInput = 0f;
    private float pitchInput = 0f;

    private XRSlider _speedSlider; // 速度滑块控制器
    private float baseSpeed = 40.0f; // 保存初始速度作为基准值

    private void Awake()
    {
        trans = GetComponent<Transform>();
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        teleportController = GetComponent<TeleportController>();
        _orbManager = GetComponent<OrbManager>();
        _lockConsole = GetComponentInChildren<LockConsole>();
        _speedConsole = GetComponentInChildren<SpeedConsole>();

        _speedSlider = GetComponentInChildren<XRSlider>();
        if (_speedSlider != null)
        {
            baseSpeed = speed; // 保存初始速度
            _speedSlider.onValueChange.AddListener(OnSpeedSliderChange);
        }

        _joystick = GetComponentInChildren<XRJoystick>();
        if (_joystick != null)
        {
            _joystick.onValueChangeX.AddListener(OnJoystickYawChange);
            _joystick.onValueChangeY.AddListener(OnJoystickPitchChange);
        }
    }

    private void Start()
    {
        teleportController.Initialize(trans);
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.freezeRotation = true;
        StartCoroutine(DelayStart());
        lastPosition = transform.position;
    }

    private IEnumerator DelayStart()
    {
        enabled = false;
        yield return new WaitForSeconds(3.5f);
        enabled = true;
    }

    void Update()
    {
        // real time speed
        float distance = Vector3.Distance(transform.position, lastPosition);
        // Calculate the speed (distance per second)
        realSpeed = distance / Time.deltaTime;
        // Update the last position
        lastPosition = transform.position;
        _speedConsole.UpdateConsole( (int)realSpeed );   
        
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
        else 
        {
            if (!teleportController.IsTeleporting())
            {
                // 计算飞船与初始方向的夹角
                float angleWithInitial = Vector3.Angle(Vector3.forward, trans.forward);
                
                // 根据夹角决定是否需要反转俯仰输入
                float adjustedPitchInput = angleWithInitial > 90f ? pitchInput : -pitchInput;
                
                // 计算每帧的旋转变化
                float pitchDelta = adjustedPitchInput * angularSpeed * Time.deltaTime;
                float yawDelta = yawInput * angularSpeed * Time.deltaTime;
                
                // 应用旋转，保持z轴旋转为90度
                trans.Rotate(pitchDelta, yawDelta, 0f, Space.World);
                Vector3 currentEuler = trans.rotation.eulerAngles;
                trans.rotation = Quaternion.Euler(currentEuler.x, currentEuler.y, 90f);
                
                // 保持速度
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

    void FixedUpdate() {
        
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
                // Debug.Log("执行传送");
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
            // Debug.Log("取消传送");
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
        // Debug.Log("飞船自旋已停止");
    }

    public void SetVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    private void OnJoystickYawChange(float value)
    {
        if (isInObservationMode)
        {
            yawInput = -value;
            // Debug.Log($"Yaw输入: {value}");
        }
    }

    private void OnJoystickPitchChange(float value)
    {
        if (isInObservationMode)
        {
            pitchInput = value;
            // Debug.Log($"Pitch输入: {value}");
        }
    }

    private void OnSpeedSliderChange(float value)
    {
        // value范围是0-1，我们将其映射到10%-100%的基准速度
        speed = baseSpeed * (0.1f + 0.9f * value);
        Debug.Log($"Speed changed to: {speed}");
    }

    private void OnDestroy()
    {
        if (_joystick != null)
        {
            _joystick.onValueChangeX.RemoveListener(OnJoystickYawChange);
            _joystick.onValueChangeY.RemoveListener(OnJoystickPitchChange);
        }

        if (_speedSlider != null)
        {
            _speedSlider.onValueChange.RemoveListener(OnSpeedSliderChange);
        }
    }

}
