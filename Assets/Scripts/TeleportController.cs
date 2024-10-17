using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public float teleportDistance = 400.0f;  // ���ô��͵ľ���
    public float teleportSpeed = 600.0f;    // ���ô����ƶ��ٶ�
    public GameObject teleportMarkerPrefab; // ���ô���Ŀ��ɴ�ģ�͵�Ԥ�Ƽ�
    public ParticleSystem teleportParticleSystem; // ����ϵͳ����
    public ParticleSystem engineParticleSystem;   // ��������Ч��
    private GameObject teleportMarker;      // ����Ŀ��ɴ�ģ�͵�ʵ��
    private bool isTeleporting = false;     // ��ʶ�Ƿ����ڴ���
    private bool teleportMode = false;      // ��ʶ�Ƿ���봫��ģʽ
    private Transform craftTransform;       // �ɴ��� Transform
    private Vector3 teleportTarget;         // ����Ŀ��λ��
    private Camera mainCamera;              // �������
    private Vector3 cameraInitialLocalPosition; // ��ǰ������ĳ�ʼ�ֲ�λ��
    public float maxFOV = 90.0f;            // ����ӳ���
    public float minFOV = 60.0f;            // ��С�ӳ��ǣ����ͽ���ʱ�ָ���
    public float shakeIntensity = 0.015f;     // ��ǿ��
    public float shakeDuration = 0.8f;      // �𶯳���ʱ��
    private float currentShakeDuration = 0f;// ��ǰ��ʱ��

    private void Start()
    {
        // ʵ��������Ŀ��ɴ�ģ�Ͳ�ʹ���ʼ����
        teleportMarker = Instantiate(teleportMarkerPrefab);
        teleportMarker.SetActive(false); // ��ʼʱ����ʾ

        // ��ȡ�������
        mainCamera = Camera.main;

        // ����������ĳ�ʼ�ֲ�λ��
        cameraInitialLocalPosition = mainCamera.transform.localPosition;

        // ֹͣ����ϵͳ��ȷ������ϵͳ��ʼʱ�ǹر�״̬��
        if (teleportParticleSystem != null)
        {
            teleportParticleSystem.Stop();
            engineParticleSystem.Stop();
        }
    }

    // ��ʼ�����Ϳ�����������ɴ��� Transform
    public void Initialize(Transform craftTransform)
    {
        this.craftTransform = craftTransform;
    }

    // ���´���Ŀ���ǵ�λ�úͳ���ʹ�����ɴ�ǰ������ɴ�����ͬ����
    public void UpdateTeleportMarkerPosition()
    {
        if (teleportMode && !isTeleporting)
        {
            // �ô���Ŀ��ģ��ʼ�ձ����ڷɴ�����ǰ�� teleportDistance ��λ��
            teleportTarget = craftTransform.position + craftTransform.forward * teleportDistance;
            teleportMarker.transform.position = teleportTarget;

            // ͬ������Ŀ��ĳ�����ɴ�һ��
            teleportMarker.transform.rotation = craftTransform.rotation;
        }
    }

    // ���봫��ģʽ�ķ���
    public void EnterTeleportMode()
    {
        if (!teleportMode)
        {
            teleportMode = true;
            teleportMarker.SetActive(true); // �����Ŀ��ģ��
            UpdateTeleportMarkerPosition(); // ��ʼʱ����һ��λ�úͳ���
        }
    }

    // ִ�д��͵ķ���
    public void StartTeleport()
    {
        if (teleportMode && !isTeleporting)
        {
            isTeleporting = true;
            teleportMode = false;
            teleportMarker.SetActive(false); // ���ش���Ŀ��ģ��

            // �������Ч��
            StartCoroutine(ApplyWideAngleEffect());

            // ������Ч��
            currentShakeDuration = shakeDuration;

            // ��������Ч��
            if (teleportParticleSystem != null)
            {
                teleportParticleSystem.Play();
                engineParticleSystem.Play();
            }
        }
    }

    // ��֡�ƶ���Ŀ�괫��λ��
    public void MoveTowardsTeleportTarget()
    {
        if (isTeleporting)
        {
            // �ɴ��ƶ���Ŀ��λ��
            craftTransform.position = Vector3.MoveTowards(craftTransform.position, teleportTarget, teleportSpeed * Time.deltaTime);

            // �������Ŀ��λ�ã���ֹͣ����
            if (Vector3.Distance(craftTransform.position, teleportTarget) < 0.1f)
            {
                isTeleporting = false;
                ResetCameraPosition(); // �ָ��������ǰ��λ��

                // ֹͣ����Ч��
                if (teleportParticleSystem != null)
                {
                    teleportParticleSystem.Stop();
                    engineParticleSystem.Stop();
                }
            }
        }

        // ִ���������Ч��
        if (currentShakeDuration > 0)
        {
            ApplyShakeEffect();
            currentShakeDuration -= Time.deltaTime;
        }
    }

    // ȡ�����͵ķ���
    public void CancelTeleport()
    {
        if (teleportMode)
        {
            teleportMode = false;
            teleportMarker.SetActive(false); // ���ش���Ŀ��ģ��

            // ֹͣ����Ч��
            if (teleportParticleSystem != null)
            {
                teleportParticleSystem.Stop();
                engineParticleSystem.Stop();
            }
        }
    }

    // �Ƿ��ڴ���ģʽ
    public bool IsInTeleportMode()
    {
        return teleportMode;
    }

    // �Ƿ����ڴ���
    public bool IsTeleporting()
    {
        return isTeleporting;
    }

    // ���Ч��Э��
    private IEnumerator ApplyWideAngleEffect()
    {
        // ���� FOV
        while (mainCamera.fieldOfView < maxFOV)
        {
            mainCamera.fieldOfView += Time.deltaTime * 55.0f; // �����ӽǱ仯�ٶ�
            yield return null;
        }

        // �ȴ����ͽ���
        while (isTeleporting)
        {
            yield return null;
        }

        // �ָ� FOV
        while (mainCamera.fieldOfView > minFOV)
        {
            mainCamera.fieldOfView -= Time.deltaTime * 65.0f;
            yield return null;
        }
    }

    // �������Ч��
    private void ApplyShakeEffect()
    {
        if (mainCamera != null)
        {
            // ���ڷɴ��ľֲ���Ч��
            Vector3 originalLocalPosition = cameraInitialLocalPosition;
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);

            // ����������ľֲ�ƫ�ƣ�����ڷɴ���
            mainCamera.transform.localPosition = new Vector3(originalLocalPosition.x + offsetX, originalLocalPosition.y + offsetY, originalLocalPosition.z);
        }
    }

    // �ָ������λ�õ���ǰ��״̬
    private void ResetCameraPosition()
    {
        // ���������λ�ûָ�����ǰ�ĳ�ʼ�ֲ�λ��
        mainCamera.transform.localPosition = cameraInitialLocalPosition;
    }
}
