using UnityEngine;

public class ShowControl : MonoBehaviour
{
    private TimeControl timeControl;

    void Start()
    {
        // 获取同一物体上的TimeControl组件
        timeControl = GetComponent<TimeControl>();
        if (timeControl == null)
        {
            Debug.LogError("ShowControl必须与TimeControl脚本挂载在同一个物体上！");
        }
    }

    // 触发显示的公共方法
    public void TriggerShow()
    {
        if (timeControl != null)
        {
            timeControl.SetShow(true);
        }
    }
}