using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftController : MonoBehaviour
{
    public float speed = 40.0f;
    public float angularSpeed = 20.0f;
    private Transform trans;
    private Camera mainCamera;
    private TeleportController teleportController;
    private bool isInObservationMode = false; // 用于切换观察模式

    // Initialization
    private void Awake()
    {
        trans = GetComponent<Transform>();
        mainCamera = trans.Find("XR Origin (VR)/Camera Offset/Main Camera").GetComponent<Camera>();
        teleportController = GetComponent<TeleportController>();
    }

    private void Start()
    {
        teleportController.Initialize(trans);  // Initialize teleport controller
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleTeleportation();
        HandleObservationMode(); // 添加处理观察模式的逻辑
    }

    // Handle the spacecraft movement based on the camera's forward direction
    private void HandleMovement()
    {
        if (!isInObservationMode) // 如果不是观察模式，飞船的方向跟随摄像头
        {
            Vector3 targetRotation = mainCamera.transform.forward;
            Quaternion rotation = Quaternion.LookRotation(targetRotation);
            trans.rotation = Quaternion.RotateTowards(trans.rotation, rotation, angularSpeed * Time.deltaTime);
        }

        // 无论是否在观察模式下，飞船都继续向前移动
        trans.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // Handle teleportation logic
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

        // Update teleport marker and move towards the teleport target, if applicable
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
}