using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftController : MonoBehaviour
{
    public float speed = 40.0f;
    public float angularSpeed = 20.0f;
    private Transform trans;
    private Camera mainCamera;
    private TeleportController teleportController; // 引用传送控制器

    // Start is called before the first frame update
    private void Awake()
    {
        trans = GetComponent<Transform>();
        mainCamera = trans.Find("Main Camera").GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 初始化传送控制器
        teleportController = GetComponent<TeleportController>();
        teleportController.Initialize(trans);
    }

    // Update is called once per frame
    void Update()
    {
        // 控制飞船的旋转
        Vector3 targetRotation = mainCamera.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(targetRotation);
        trans.rotation = Quaternion.RotateTowards(trans.rotation, rotation, angularSpeed * Time.deltaTime);

        // 控制飞船的正常移动
        trans.Translate(Vector3.forward * speed * Time.deltaTime);

        // 控制传送模式
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!teleportController.IsInTeleportMode())
            {
                teleportController.EnterTeleportMode(); // 进入传送模式
            }
            else if (!teleportController.IsTeleporting())
            {
                teleportController.StartTeleport(); // 执行传送
            }
        }

        // 取消传送
        if (Input.GetKeyDown(KeyCode.Q) && teleportController.IsInTeleportMode())
        {
            teleportController.CancelTeleport(); // 取消传送
        }

        // 更新传送目标位置
        teleportController.UpdateTeleportMarkerPosition();

        // 执行传送移动
        teleportController.MoveTowardsTeleportTarget();
    }
}
