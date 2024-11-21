using UnityEngine;
using UnityEngine.UI;

public class MiniMapMarkerManager : MonoBehaviour
{
    public Camera miniMapCamera; // 小地图相机
    public RectTransform miniMapImage; // 小地图 UI 图像
    public GameObject markerPrefab; // 标记图标预制体

    private RectTransform markerRectTransform;

    void Start()
    {
        // 创建标记图标实例
        GameObject markerInstance = Instantiate(markerPrefab, miniMapImage);
        markerRectTransform = markerInstance.GetComponent<RectTransform>();
        markerRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        markerRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        // 更新标记图标的位置
        Vector3 worldPosition = transform.position;
        Vector3 viewportPosition = miniMapCamera.WorldToViewportPoint(worldPosition);
        Vector2 miniMapSize = miniMapImage.sizeDelta;

        // 将视口坐标转换为小地图坐标
        Vector2 miniMapPosition = new Vector2(
            (viewportPosition.x - 0.5f) * miniMapSize.x * 2f,
            (viewportPosition.y - 0.5f) * miniMapSize.y * 2f
        );

        markerRectTransform.anchoredPosition = miniMapPosition;
    }
}