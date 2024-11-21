using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource S1;
    [SerializeField] private AudioSource S2;
    [SerializeField] private AudioSource S3;
    
    private Collider triggerCollider;

    private void Awake()
    {
        // 获取物体上的Collider组件
        triggerCollider = GetComponent<Collider>();
        
        // 确保有Collider组件且设置为触发器
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
        else
        {
            Debug.LogWarning("没有找到Collider组件，请添加Collider组件到物体上！");
        }
    }

    // 将OnCollisionEnter改为OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞物体的标签并触发相应的音频
        switch (other.gameObject.tag)
        {
            case "Obstacle":
                if (S1 != null)
                    S1.Play();
                break;
            
            case "EnergyOrb":
                if (S2 != null)
                    S2.Play();
                break;
            
            case "XXX":
                if (S3 != null)
                    S3.Play();
                break;
        }
    }
}