using UnityEngine;

public class AudioPress : MonoBehaviour
{
    [Header("音频源设置")]
    [SerializeField] private AudioSource S1;
    [SerializeField] private AudioSource S2;
    [SerializeField] private AudioSource S3;

    [Header("按键设置")]
    [SerializeField] private KeyCode K1 = KeyCode.Alpha1;  // 默认为数字键1
    [SerializeField] private KeyCode K2 = KeyCode.Alpha2;  // 默认为数字键2
    [SerializeField] private KeyCode K3 = KeyCode.Alpha3;  // 默认为数字键3

    void Update()
    {
        // 检测K1按键
        if (Input.GetKeyDown(K1))
        {
            if (S1 != null)
                S1.Play();
        }

        // 检测K2按键
        if (Input.GetKeyDown(K2))
        {
            if (S2 != null)
                S2.Play();
        }

        // 检测K3按键
        if (Input.GetKeyDown(K3))
        {
            if (S3 != null)
                S3.Play();
        }
    }
}