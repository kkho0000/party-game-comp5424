using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWall : MonoBehaviour
{
    [SerializeField]
    private GameObject targetText;
    [SerializeField]
    private Transform teleportPosition;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private Transform object3Transform;
    [SerializeField]
    private Transform lookAtTarget;
    [SerializeField]
    private float freezeDuration = 2f;
    
    private ShowControl showControl;
    private Rigidbody parentRigidbody;

    void Start()
    {
        // 检查必要组件
        if (targetText == null || teleportPosition == null || 
            object3Transform == null || lookAtTarget == null)
        {
            // Debug.LogError("请在Inspector中关联所有必要对象！");
            return;
        }

        showControl = targetText.GetComponent<ShowControl>();
        if (showControl == null)
        {
            // Debug.LogError("目标文本对象上必须挂载ShowControl脚本！");
        }

        parentRigidbody = transform.root.GetComponent<Rigidbody>();
        if (parentRigidbody == null)
        {
            // Debug.LogWarning("父物体上未找到Rigidbody组件！");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            ResetToCheckpoint();
        }
    }

    private void ResetToCheckpoint()
    {
        // 重置父物体位置
        Transform rootTransform = transform.root;
        rootTransform.position = teleportPosition.position;
        
        // 重置关联物体3的旋转并朝向目标点
        if (object3Transform != null && lookAtTarget != null)
        {
            object3Transform.rotation = Quaternion.Euler(0, 0, -90);
            object3Transform.LookAt(lookAtTarget);
        }
        
        // 触发文本显示
        if (showControl != null)
        {
            showControl.TriggerShow();
        }

        // 开始临时静态协程
        StartCoroutine(TemporaryKinematicCoroutine());
    }

    private IEnumerator TemporaryKinematicCoroutine()
    {
        if (parentRigidbody != null)
        {
            parentRigidbody.isKinematic = true;
        }

        yield return new WaitForSeconds(freezeDuration);

        if (parentRigidbody != null)
        {
            parentRigidbody.isKinematic = false;
        }
    }
}
