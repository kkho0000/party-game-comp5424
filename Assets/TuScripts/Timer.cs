using UnityEngine;

public class Timer : MonoBehaviour
{
    // 在Inspector中可调整的开始时间（秒）
    [SerializeField]
    private int startTime = 0;

    // 全局计时器
    private float globalTimer = 0f;

    // ShowControl组件引用
    private ShowControl showControl;

    // 用于确保TriggerShow只被调用一次的标志
    private bool hasTriggered = false;

    void Start()
    {
        // 获取同一物体上的ShowControl组件
        showControl = GetComponent<ShowControl>();
        if (showControl == null)
        {
            Debug.LogError("Timer必须与ShowControl脚本挂载在同一个物体上！");
        }
    }

    void Update()
    {
        // 更新全局计时器
        globalTimer += Time.deltaTime;

        // 检查是否达到启动时间且尚未触发
        if (!hasTriggered && globalTimer >= startTime)
        {
            if (showControl != null)
            {
                showControl.TriggerShow();
                hasTriggered = true;
            }
        }
    }
}