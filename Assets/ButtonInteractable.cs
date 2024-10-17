using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ButtonInteractable : XRBaseInteractable
{
    [SerializeField] Transform buttonTransform;
    float initHeight;

    float offset;

    private void Start()
    {
        initHeight = GetButtonHeight(buttonTransform.position);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hoverEntered.AddListener(StartPush);
        hoverExited.AddListener(StopPush);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        hoverEntered.RemoveListener(StartPush);
        hoverExited.RemoveListener(StopPush);
    }

    void StartPush(HoverEnterEventArgs args)
    {
        var interactor = args.interactorObject;
        var interactorAttachPose = interactor.GetAttachTransform(this).GetWorldPose();

        offset = GetButtonHeight(interactorAttachPose.position);
        offset -= GetButtonHeight(buttonTransform.position);
    }

    void StopPush(HoverExitEventArgs args)
    {
        SetButtonHeight(initHeight);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        switch (updatePhase)
        {
            case XRInteractionUpdateOrder.UpdatePhase.Dynamic:
                UpdateTarget();
                break;
        }
    }

    private void UpdateTarget()
    {
        if (isHovered)
        {
            var interactor = interactorsHovering[0];
            var interactorAttachPose = interactor.GetAttachTransform(this).GetWorldPose();

            float y = GetButtonHeight(interactorAttachPose.position);
            SetButtonHeight(y - offset);
        }
    }

    private void SetButtonHeight(float y)
    {
        y = Mathf.Clamp(y, 0, initHeight);
        buttonTransform.position = transform.position + transform.up * y;
    }

    private float GetButtonHeight(Vector3 pos)
    {
        return Vector3.Dot(transform.up, pos - transform.position);
    }
}