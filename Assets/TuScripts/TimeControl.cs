using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 添加 TextMeshPro 引用

public class TimeControl : MonoBehaviour
{
    [SerializeField] private int showTime = 3; // 文本展示时间，默认3秒
    [SerializeField] private int order = 0;    // 文本展示顺序
    [SerializeField] private bool triggerNextText = false; // 新增开关
    [SerializeField] private GameObject nextTextObject;    // 新增关联对象
    
    private bool show = false;                 // 控制展示的开关
    private TextMeshProUGUI textComponent;     // 文本组件引用
    private float currentShowTime;             // 当前展示时间计数器
    private bool isWaitingForNext = false;     // 新增等待标志
    private ShowControl nextShowControl;        // 新增ShowControl引用

    void Start()
    {
        // 获取文本组件
        textComponent = GetComponent<TextMeshProUGUI>();
        if (textComponent == null)
        {
            Debug.LogError("TimeControl脚本必须挂载在带有TextMeshProUGUI组件的对象上！");
            return;
        }
        
        // 获取关联对象的ShowControl组件
        if (triggerNextText && nextTextObject != null)
        {
            nextShowControl = nextTextObject.GetComponent<ShowControl>();
            if (nextShowControl == null)
            {
                Debug.LogError("关联对象必须挂载ShowControl组件！");
            }
        }
        
        // 初始化时隐藏文本
        textComponent.enabled = false;
    }

    void Update()
    {
        if (show)
        {
            // 如果处于显示状态，开始计时
            currentShowTime += Time.deltaTime;
            
            // 如果显示时间达到设定值，则隐藏文本
            if (currentShowTime >= showTime)
            {
                show = false;
                textComponent.enabled = false;
                currentShowTime = 0;
                
                // 如果开关开启且有有效的关联对象，开始等待
                if (triggerNextText && nextShowControl != null)
                {
                    isWaitingForNext = true;
                    StartCoroutine(TriggerNextTextCoroutine());
                }
            }
        }
    }

    private IEnumerator TriggerNextTextCoroutine()
    {
        // 等待2秒
        yield return new WaitForSeconds(2f);
        
        // 触发下一个文本
        if (isWaitingForNext && nextShowControl != null)
        {
            nextShowControl.TriggerShow();
            isWaitingForNext = false;
        }
    }

    // 接收来自ShowControl的信号
    public void SetShow(bool value)
    {
        if (value && !show)
        {
            show = true;
            textComponent.enabled = true;
            currentShowTime = 0;
            isWaitingForNext = false; // 重置等待标志
        }
    }

    // 获取展示顺序
    public int GetOrder()
    {
        return order;
    }
}