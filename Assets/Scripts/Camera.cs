using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public float mouseSensitivity = 200;
    public Vector2 cameraAngleLimit = new Vector2(-20, 20);
    private Vector3 cameraAngle;
    private Vector3 craftAngle;

    private bool isAligningWithSpaceCraft = false; // 标志摄像头是否正在对齐飞船
    private Transform spaceCraftTransform; // 飞船的Transform
    public float rotationSpeed = 5.0f; // 摄像头对齐飞船的速度

    // Start is called before the first frame update
    private void Awake()
    {
        cameraAngle = GetComponent<Transform>().eulerAngles;
        craftAngle = GetComponent<Transform>().parent.eulerAngles;
    }

    // 绑定飞船的Transform
    public void SetSpaceCraftTransform(Transform craftTransform)
    {
        spaceCraftTransform = craftTransform;
    }

    // 启动摄像头与飞船方向对齐的功能
    public void AlignWithSpaceCraft()
    {
        isAligningWithSpaceCraft = true;
    }

    // 停止摄像头对齐飞船的功能
    public void StopAligning()
    {
        isAligningWithSpaceCraft = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAligningWithSpaceCraft && spaceCraftTransform != null)
        {
            // 逐渐对齐摄像头的方向与飞船方向
            Quaternion targetRotation = Quaternion.LookRotation(spaceCraftTransform.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // 当摄像头的旋转与飞船的方向足够接近时，停止对齐
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isAligningWithSpaceCraft = false; // 对齐完成，关闭标志
            }
        }
        else
        {
            // 普通视角控制逻辑
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            cameraAngle.x -= mouseY;
            cameraAngle.y += mouseX;

            // 限制摄像头旋转角度
            cameraAngle.x = Mathf.Clamp(cameraAngle.x, cameraAngleLimit.x, cameraAngleLimit.y);

            GetComponent<Transform>().eulerAngles = cameraAngle;
        }
    }
}
