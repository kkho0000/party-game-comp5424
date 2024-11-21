using UnityEngine;

public class TriggerImpact : MonoBehaviour
{
    [SerializeField]
    private GameObject targetText;
    [SerializeField]
    private GameObject targetText2;
    [SerializeField]
    private GameObject targetText3;
    
    private ShowControl showControl;
    private ShowControl showControl2;
    private ShowControl showControl3;
    
    private bool hasTriggered = false;
    private bool hasTriggered2 = false;
    private int energyOrbCollisions = 0;

    void Start()
    {
        if (targetText == null || targetText2 == null || targetText3 == null)
        {
            // Debug.LogError("请在Inspector中关联所有目标文本对象！");
            return;
        }

        showControl = targetText.GetComponent<ShowControl>();
        showControl2 = targetText2.GetComponent<ShowControl>();
        showControl3 = targetText3.GetComponent<ShowControl>();
        
        if (showControl == null || showControl2 == null || showControl3 == null)
        {
            // Debug.LogError("目标文本对象上必须挂载ShowControl脚本！");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyOrb"))
        {
            if (!hasTriggered)
            {
                showControl?.TriggerShow();
                hasTriggered = true;
            }
            
            energyOrbCollisions++;
            
            if (energyOrbCollisions == 4)
            {
                showControl3?.TriggerShow();
            }
        }
        
        if (!hasTriggered2 && other.CompareTag("Obstacle"))
        {
            showControl2?.TriggerShow();
            hasTriggered2 = true;
        }
    }
}