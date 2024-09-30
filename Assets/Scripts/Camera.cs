using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float mouseSensitivity;
    public Vector2 cameraAngleLimit = new Vector2();
    private Vector3 cameraAngle;
    private Vector3 craftAngle;
    
    // Start is called before the first frame update
    private void Awake() {
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
        // cameraAngle.x = Mathf.Clamp(cameraAngle.x, cameraAngleLimit.x, cameraAngleLimit.y);
        // cameraAngle.y = Mathf.Clamp(cameraAngle.y, cameraAngleLimit.x, cameraAngleLimit.y);

        GetComponent<Transform>().eulerAngles = cameraAngle;
    }
}
