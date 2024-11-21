using UnityEngine;
using System.IO;

public class CaptureCameraImage : MonoBehaviour
{
    public Camera targetCamera; // 目标相机
    public int imageWidth = 1920; // 图像宽度
    public int imageHeight = 1080; // 图像高度
    public string savePath = "CapturedImage.png"; // 保存路径

    void Start()
    {
        // 创建 RenderTexture
        RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
        targetCamera.targetTexture = renderTexture;

        // 渲染相机
        targetCamera.Render();

        // 创建 Texture2D
        Texture2D texture = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);

        // 读取 RenderTexture 的数据到 Texture2D
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
        texture.Apply();

        // 重置相机的目标纹理和 RenderTexture.active
        targetCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        // 将 Texture2D 保存为 PNG 文件
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(Application.dataPath, savePath), bytes);

        Debug.Log("图片已保存到: " + Path.Combine(Application.dataPath, savePath));
    }
}