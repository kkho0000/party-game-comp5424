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
    private JumptoPlay jumptoPlay;

    void Awake()
    {
        spaceCraftController = FindObjectOfType<SpaceCraftController>();
        jumptoPlay = FindObjectOfType<JumptoPlay>();
    }

    void Start()
    {
        //使用右手手柄A键 实现传送
        teleport = testActionAsset.FindAction("Teleport");
        teleport.performed += Teleport;

        //使用右手手柄B键 实现取消传送
        cancelTeleport = testActionAsset.FindAction("CancelTeleport");
        cancelTeleport.performed += CancelTeleport;

        //使用左手手柄X键 实现重置
        movementReset = testActionAsset.FindAction("Reset");
        movementReset.performed += ResetSpaceship;

        //使用左手手柄Y键 实现观察模式（解锁）
        observeMode = testActionAsset.FindAction("ObserveMode");
        observeMode.started += SwitchMode;
    }

    /*
    private void StartReverse(InputAction.CallbackContext context)
    {
        Debug.Log("执行倒车");
        spaceCraftController.isReversing = true;
    }
    private void StopReverse(InputAction.CallbackContext context)
    {
        Debug.Log("停止倒车");
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
    }

   
}

