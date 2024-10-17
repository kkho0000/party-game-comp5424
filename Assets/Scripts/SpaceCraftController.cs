using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftController : MonoBehaviour
{
    public float speed = 40.0f;
    public float angularSpeed = 20.0f;
    private Transform trans;
    private Camera mainCamera;
    private TeleportController teleportController; // ���ô��Ϳ�����

    // Start is called before the first frame update
    private void Awake()
    {
        trans = GetComponent<Transform>();
        mainCamera = trans.Find("Main Camera").GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // ��ʼ�����Ϳ�����
        teleportController = GetComponent<TeleportController>();
        teleportController.Initialize(trans);
    }

    // Update is called once per frame
    void Update()
    {
        // ���Ʒɴ�����ת
        Vector3 targetRotation = mainCamera.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(targetRotation);
        trans.rotation = Quaternion.RotateTowards(trans.rotation, rotation, angularSpeed * Time.deltaTime);

        // ���Ʒɴ��������ƶ�
        trans.Translate(Vector3.forward * speed * Time.deltaTime);

        // ���ƴ���ģʽ
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!teleportController.IsInTeleportMode())
            {
                teleportController.EnterTeleportMode(); // ���봫��ģʽ
            }
            else if (!teleportController.IsTeleporting())
            {
                teleportController.StartTeleport(); // ִ�д���
            }
        }

        // ȡ������
        if (Input.GetKeyDown(KeyCode.Q) && teleportController.IsInTeleportMode())
        {
            teleportController.CancelTeleport(); // ȡ������
        }

        // ���´���Ŀ��λ��
        teleportController.UpdateTeleportMarkerPosition();

        // ִ�д����ƶ�
        teleportController.MoveTowardsTeleportTarget();
    }
}
