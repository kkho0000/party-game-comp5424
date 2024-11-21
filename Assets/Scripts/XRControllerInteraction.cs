using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class XRControllerInteraction : MonoBehaviour
{
    public InputActionAsset testActionAsset;

    private InputAction movementReset;
    private InputAction teleport;
    private InputAction cancelTeleport;
    private InputAction observeMode;

    private SpaceCraftController spaceCraftController;

    void Awake()
    {
        spaceCraftController = FindObjectOfType<SpaceCraftController>();
    }

    void Start()
    {
        //ʹ�������ֱ�A�� ʵ�ִ���
        teleport = testActionAsset.FindAction("Teleport");
        teleport.performed += Teleport;

        //ʹ�������ֱ�B�� ʵ��ȡ������
        cancelTeleport = testActionAsset.FindAction("CancelTeleport");
        cancelTeleport.performed += CancelTeleport;

        //ʹ�������ֱ�X�� ʵ������
        movementReset = testActionAsset.FindAction("Reset");
        movementReset.performed += ResetSpaceship;

        //ʹ�������ֱ�Y�� ʵ�ֹ۲�ģʽ��������
        observeMode = testActionAsset.FindAction("ObserveMode");
        observeMode.started += SwitchMode;
    }

    /*
    private void StartReverse(InputAction.CallbackContext context)
    {
        Debug.Log("ִ�е���");
        spaceCraftController.isReversing = true;
    }
    private void StopReverse(InputAction.CallbackContext context)
    {
        Debug.Log("ֹͣ����");
        spaceCraftController.isReversing = false;
    }
    */

    private void ResetSpaceship(InputAction.CallbackContext context)
    {
        spaceCraftController.ResetSpaceship();
    }

    private void Teleport(InputAction.CallbackContext context)
    {
        spaceCraftController.startTele();
    }

    private void CancelTeleport(InputAction.CallbackContext context)
    {
        spaceCraftController.cancelTele();
    }
    private void SwitchMode(InputAction.CallbackContext context)
    {
        spaceCraftController.SwitchObservationMode();
        // Debug.Log("观察者模式");
    }

   
}

