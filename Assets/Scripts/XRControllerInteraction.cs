using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRControllerInteraction : MonoBehaviour
{
  
    public XRRayInteractor controller;
    private SpaceCraftController spaceCraftController;
    private JumptoPlay jumptoPlay;

    void Start()
    {
        //controller = GetComponent<XRRayInteractor>();
        spaceCraftController = FindObjectOfType<SpaceCraftController>();
        jumptoPlay = FindObjectOfType<JumptoPlay>();
    }

    void Update()
    {
        if (controller)
        {
            if(controller==null)
            {
                Debug.Log("controller is null");
            }
            RaycastHit hit;
            bool hitSomething = controller.TryGetCurrent3DRaycastHit(out hit);

            if (hitSomething)
            {
                Collider collider = hit.collider;
                Debug.Log("I hit something: " + collider.name);

                //if (Input.GetButtonDown("Fire1")) // ����Ƿ��������������ֱ��Ĵ�����ť
                //{
                    if (collider.CompareTag("ResetButton"))
                    {
                        Debug.Log("Interacted with: " + collider.gameObject.name);
                        // ����SpaceCraftController�ĺ���
                        spaceCraftController.ResetSpaceship();
                    }
                    else if (collider.CompareTag("BackButton"))
                    {
                        Debug.Log("Interacted with: " + collider.gameObject.name);
                        // ����SpaceCraftController�ĺ���
                        jumptoPlay.LoadScene("UI");
                }
                //}
            }
        }
    }
}

