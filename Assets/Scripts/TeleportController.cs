using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public float teleportDistance = 400.0f;  // 设置传送的距离
    public float teleportSpeed = 600.0f;    // 设置传送移动速度
    public GameObject teleportMarkerPrefab; // 引用传送目标飞船模型的预制件
    public ParticleSystem teleportParticleSystem; // 粒子系统引用
    public ParticleSystem engineParticleSystem;   // 引擎粒子效果
    private GameObject teleportMarker;      // 传送目标飞船模型的实例
    private bool isTeleporting = false;     // 标识是否正在传送
    private bool teleportMode = false;      // 标识是否进入传送模式
    private Transform craftTransform;       // 飞船的 Transform
    private Vector3 teleportTarget;         // 传送目标位置
    private Camera mainCamera;              // 主摄像机
    private Vector3 cameraInitialLocalPosition; // 震动前摄像机的初始局部位置
    public float maxFOV = 90.0f;            // 最大视场角
    public float minFOV = 60.0f;            // 最小视场角（传送结束时恢复）
    public float shakeIntensity = 0.015f;     // 震动强度
    public float shakeDuration = 0.8f;      // 震动持续时间
    private float currentShakeDuration = 0f;// 当前震动时间

    private void Start()
    {
        // 实例化传送目标飞船模型并使其最开始隐藏
        teleportMarker = Instantiate(teleportMarkerPrefab);
        teleportMarker.SetActive(false); // 开始时不显示

        // 获取主摄像机
        mainCamera = Camera.main;

        // 保存摄像机的初始局部位置
        cameraInitialLocalPosition = mainCamera.transform.localPosition;

        // 停止粒子系统（确保粒子系统开始时是关闭状态）
        if (teleportParticleSystem != null)
        {
            teleportParticleSystem.Stop();
            engineParticleSystem.Stop();
        }
    }

    // 初始化传送控制器，传入飞船的 Transform
    public void Initialize(Transform craftTransform)
    {
        this.craftTransform = craftTransform;
    }

    // 更新传送目标标记的位置和朝向（使其跟随飞船前方并与飞船朝向同步）
    public void UpdateTeleportMarkerPosition()
    {
        if (teleportMode && !isTeleporting)
        {
            // 让传送目标模型始终保持在飞船的正前方 teleportDistance 的位置
            teleportTarget = craftTransform.position + craftTransform.forward * teleportDistance;
            teleportMarker.transform.position = teleportTarget;

            // 同步传送目标的朝向与飞船一致
            teleportMarker.transform.rotation = craftTransform.rotation;
        }
    }

    // 进入传送模式的方法
    public void EnterTeleportMode()
    {
        if (!teleportMode)
        {
            teleportMode = true;
            teleportMarker.SetActive(true); // 激活传送目标模型
            UpdateTeleportMarkerPosition(); // 初始时更新一次位置和朝向
        }
    }

    // 执行传送的方法
    public void StartTeleport()
    {
        if (teleportMode && !isTeleporting)
        {
            isTeleporting = true;
            teleportMode = false;
            teleportMarker.SetActive(false); // 隐藏传送目标模型

            // 启动广角效果
            StartCoroutine(ApplyWideAngleEffect());

            // 启动震动效果
            currentShakeDuration = shakeDuration;

            // 启动粒子效果
            if (teleportParticleSystem != null)
            {
                teleportParticleSystem.Play();
                engineParticleSystem.Play();
            }
        }
    }

    // 逐帧移动到目标传送位置
    public void MoveTowardsTeleportTarget()
    {
        if (isTeleporting)
        {
            // 飞船移动到目标位置
            craftTransform.position = Vector3.MoveTowards(craftTransform.position, teleportTarget, teleportSpeed * Time.deltaTime);

            // 如果到达目标位置，则停止传送
            if (Vector3.Distance(craftTransform.position, teleportTarget) < 0.1f)
            {
                isTeleporting = false;
                ResetCameraPosition(); // 恢复摄像机震动前的位置

                // 停止粒子效果
                if (teleportParticleSystem != null)
                {
                    teleportParticleSystem.Stop();
                    engineParticleSystem.Stop();
                }
            }
        }

        // 执行摄像机震动效果
        if (currentShakeDuration > 0)
        {
            ApplyShakeEffect();
            currentShakeDuration -= Time.deltaTime;
        }
    }

    // 取消传送的方法
    public void CancelTeleport()
    {
        if (teleportMode)
        {
            teleportMode = false;
            teleportMarker.SetActive(false); // 隐藏传送目标模型

            // 停止粒子效果
            if (teleportParticleSystem != null)
            {
                teleportParticleSystem.Stop();
                engineParticleSystem.Stop();
            }
        }
    }

    // 是否处于传送模式
    public bool IsInTeleportMode()
    {
        return teleportMode;
    }

    // 是否正在传送
    public bool IsTeleporting()
    {
        return isTeleporting;
    }

    // 广角效果协程
    private IEnumerator ApplyWideAngleEffect()
    {
        // 增加 FOV
        while (mainCamera.fieldOfView < maxFOV)
        {
            mainCamera.fieldOfView += Time.deltaTime * 55.0f; // 调整视角变化速度
            yield return null;
        }

        // 等待传送结束
        while (isTeleporting)
        {
            yield return null;
        }

        // 恢复 FOV
        while (mainCamera.fieldOfView > minFOV)
        {
            mainCamera.fieldOfView -= Time.deltaTime * 65.0f;
            yield return null;
        }
    }

    // 摄像机震动效果
    private void ApplyShakeEffect()
    {
        if (mainCamera != null)
        {
            // 基于飞船的局部震动效果
            Vector3 originalLocalPosition = cameraInitialLocalPosition;
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);

            // 设置摄像机的局部偏移（相对于飞船）
            mainCamera.transform.localPosition = new Vector3(originalLocalPosition.x + offsetX, originalLocalPosition.y + offsetY, originalLocalPosition.z);
        }
    }

    // 恢复摄像机位置到震动前的状态
    private void ResetCameraPosition()
    {
        // 将摄像机的位置恢复到震动前的初始局部位置
        mainCamera.transform.localPosition = cameraInitialLocalPosition;
    }
}
