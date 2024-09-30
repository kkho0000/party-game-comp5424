using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCraftController : MonoBehaviour
{
    public float speed = 10.0f;
    public float angularSpeed = 5.0f;
    private Transform trans;
    private Camera mainCamera;
    // Start is called before the first frame update
    private void Awake() {
        trans = GetComponent<Transform>();
        mainCamera = trans.Find("Main Camera").GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetRotation = mainCamera.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(targetRotation);
        trans.rotation = Quaternion.RotateTowards(trans.rotation, rotation, angularSpeed * Time.deltaTime);
        trans.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
