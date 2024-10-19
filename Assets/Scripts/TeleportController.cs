using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    public float teleportDistance = 400.0f;
    public float teleportSpeed = 600.0f;
    public GameObject teleportMarkerPrefab;
    public ParticleSystem teleportParticleSystem;
    public ParticleSystem engineParticleSystem;
    private GameObject teleportMarker;
    private bool isTeleporting = false;
    private bool teleportMode = false;
    private Transform craftTransform;
    private Vector3 teleportTarget;
    private Camera mainCamera;
    private Vector3 cameraInitialLocalPosition;
    public float maxFOV = 90.0f;
    public float minFOV = 60.0f;
    public float shakeIntensity = 0.015f;
    public float shakeDuration = 0.8f;
    private float currentShakeDuration = 0f;

    private void Start()
    {
        teleportMarker = Instantiate(teleportMarkerPrefab);
        teleportMarker.SetActive(false);

        mainCamera = Camera.main;

        cameraInitialLocalPosition = mainCamera.transform.localPosition;

        if (teleportParticleSystem != null)
        {
            teleportParticleSystem.Stop();
            engineParticleSystem.Stop();
        }
    }

    public void Initialize(Transform craftTransform)
    {
        this.craftTransform = craftTransform;
    }

    public void UpdateTeleportMarkerPosition()
    {
        if (teleportMode && !isTeleporting)
        {
            teleportTarget = craftTransform.position + craftTransform.forward * teleportDistance;
            teleportMarker.transform.position = teleportTarget;

            teleportMarker.transform.rotation = craftTransform.rotation;
        }
    }

    public void EnterTeleportMode()
    {
        if (!teleportMode)
        {
            teleportMode = true;
            teleportMarker.SetActive(true);
            UpdateTeleportMarkerPosition();
        }
    }

    public void StartTeleport()
    {
        if (teleportMode && !isTeleporting)
        {
            isTeleporting = true;
            teleportMode = false;
            teleportMarker.SetActive(false);

            StartCoroutine(ApplyWideAngleEffect());

            currentShakeDuration = shakeDuration;

            if (teleportParticleSystem != null)
            {
                teleportParticleSystem.Play();
                engineParticleSystem.Play();
            }
        }
    }

    public void MoveTowardsTeleportTarget()
    {
        if (isTeleporting)
        {
            craftTransform.position = Vector3.MoveTowards(craftTransform.position, teleportTarget, teleportSpeed * Time.deltaTime);

            if (Vector3.Distance(craftTransform.position, teleportTarget) < 0.1f)
            {
                isTeleporting = false;
                ResetCameraPosition();

                if (teleportParticleSystem != null)
                {
                    teleportParticleSystem.Stop();
                    engineParticleSystem.Stop();
                }
            }
        }

        if (currentShakeDuration > 0)
        {
            ApplyShakeEffect();
            currentShakeDuration -= Time.deltaTime;
        }
    }

    public void CancelTeleport()
    {
        if (teleportMode)
        {
            teleportMode = false;
            teleportMarker.SetActive(false);

            if (teleportParticleSystem != null)
            {
                teleportParticleSystem.Stop();
                engineParticleSystem.Stop();
            }
        }
    }

    public bool IsInTeleportMode()
    {
        return teleportMode;
    }

    public bool IsTeleporting()
    {
        return isTeleporting;
    }

    private IEnumerator ApplyWideAngleEffect()
    {
        while (mainCamera.fieldOfView < maxFOV)
        {
            mainCamera.fieldOfView += Time.deltaTime * 55.0f;
            yield return null;
        }

        while (isTeleporting)
        {
            yield return null;
        }

        while (mainCamera.fieldOfView > minFOV)
        {
            mainCamera.fieldOfView -= Time.deltaTime * 65.0f;
            yield return null;
        }
    }

    private void ApplyShakeEffect()
    {
        if (mainCamera != null)
        {
            Vector3 originalLocalPosition = cameraInitialLocalPosition;
            float offsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float offsetY = Random.Range(-shakeIntensity, shakeIntensity);

            mainCamera.transform.localPosition = new Vector3(originalLocalPosition.x + offsetX, originalLocalPosition.y + offsetY, originalLocalPosition.z);
        }
    }

    private void ResetCameraPosition()
    {
        mainCamera.transform.localPosition = cameraInitialLocalPosition;
    }
}
