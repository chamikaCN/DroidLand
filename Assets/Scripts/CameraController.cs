using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Singleton

    public static CameraController instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("many cameracontollers");
            return;
        }

        instance = this;
    }
    #endregion
    public Transform target;
    public Vector3 offset;

    [Range(0.0f, 100.0f)]
    public float maxZoom, minZoom, zoomSpeed, yawSpeed, updownSpeed;
    
    float currrentZoom, currentYaw, pitch, currentUpdown;

    void Start(){
        currrentZoom = 10f; 
        currentYaw = 1f; 
        pitch = 2f; 
        currentUpdown = 1f;
    }

    void Update()
    {
        //currrentZoom -= Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
        //currrentZoom = Mathf.Clamp(currrentZoom, minZoom, maxZoom);

        //CalculateCameraMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

    }
    void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
        transform.RotateAround(target.position, transform.right, currentUpdown);
    }

    public void setTransform(Transform transform)
    {
        target = transform;

    }

    public void CalculateCameraMovement(float horizontal, float vertical){
        currentYaw -= horizontal * yawSpeed * Time.deltaTime;
        currentUpdown -= vertical * updownSpeed * Time.deltaTime;
    }

}
