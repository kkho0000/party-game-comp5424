using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCamera : MonoBehaviour
{
    public float mouseSensitivity = 200;
    public Vector2 cameraAngleLimit = new Vector2(-20, 20);
    private Vector3 cameraAngle;
    private Vector3 craftAngle;

    // Start is called before the first frame update
    private void Awake()
    {
        cameraAngle = GetComponent<Transform>().eulerAngles;
        craftAngle = GetComponent<Transform>().parent.eulerAngles;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        cameraAngle.x -= mouseY;
        cameraAngle.y += mouseX;

        GetComponent<Transform>().eulerAngles = cameraAngle;
    }
}
