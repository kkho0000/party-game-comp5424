using UnityEngine;
using UnityEngine.InputSystem;

public class PressTrigger : MonoBehaviour
{
    [SerializeField] private GameObject targetText;       // 关联的文本对象
    [SerializeField] private Key triggerKey = Key.Space;  // 键盘按键
    
    private ShowControl showControl;
    private bool isTriggered = false;
    private bool hasBeenTriggered = false;  // 新增：是否已经触发过
    private float triggerTimer = 0f;
    private const float TRIGGER_DELAY = 1f;  // 1秒延迟

    void Start()
    {
        if (targetText == null)
        {
            Debug.LogError("请设置目标文本对象！");
            return;
        }

        showControl = targetText.GetComponent<ShowControl>();
        if (showControl == null)
        {
            Debug.LogError("目标文本必须包含ShowControl组件！");
        }
    }

    void Update()
    {
        // 如果已经触发过，直接返回
        if (hasBeenTriggered)
            return;

        // 检测键盘输入
        bool keyPressed = Keyboard.current[triggerKey].wasPressedThisFrame;

        // 如果按下设定按键且未处于触发状态
        if (keyPressed && !isTriggered)
        {
            isTriggered = true;
            triggerTimer = 0f;
        }

        // 如果处于触发状态，开始计时
        if (isTriggered)
        {
            triggerTimer += Time.deltaTime;
            
            // 到达延迟时间后触发
            if (triggerTimer >= TRIGGER_DELAY)
            {
                if (showControl != null)
                {
                    showControl.TriggerShow();
                    hasBeenTriggered = true;  // 标记已触发过
                }
                isTriggered = false;
            }
        }
    }
}
