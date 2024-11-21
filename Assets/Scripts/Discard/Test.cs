using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    public const int generateCount = 100;
    public float offsetScaleMin = 200.0f;
    public float offsetScaleMax = 300.0f;
    public float rockScale = 10.0f;
    public GameObject tube;
    public GameObject prefab;
    MeshFilter meshFilter;
    Mesh mesh;
    
    Vector3[] vertices = new Vector3[generateCount];
    // public GameObject tube;
    void Awake()
    {
        // 获取 MeshFilter 组件
        meshFilter = tube.GetComponent<MeshFilter>();
        mesh = meshFilter.sharedMesh;
        if (mesh != null)
        {
            // 获取网格
            for ( int i = 0; i < generateCount; i ++ )
            {
                Vector3 offset = new Vector3(Random.Range(offsetScaleMin, offsetScaleMax), Random.Range(offsetScaleMin, offsetScaleMax), Random.Range(offsetScaleMin, offsetScaleMax));
                Vector3 alpha = new Vector3(Random.Range(0.0f, 1.0f)>0.5f?1:-1, Random.Range(0.0f, 1.0f)>0.5f?1:-1, Random.Range(0.0f, 1.0f)>0.5f?1:-1);
                vertices[i] = mesh.vertices[Random.Range(0, mesh.vertices.Length)];
                vertices[i] = tube.transform.TransformPoint(vertices[i]);
                vertices[i] += Vector3.Scale(offset, alpha);
                // Debug.Log(vertices[i]);
            }
        }
        else
        {
            Debug.LogError("没有找到 MeshFilter 组件");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        for ( int i = 0; i < generateCount; i ++ )
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = vertices[i];
            go.transform.localScale = Vector3.one * rockScale;
            go.name = "Rock" + i;
            go.transform.SetParent(transform);
        }
    }
}
