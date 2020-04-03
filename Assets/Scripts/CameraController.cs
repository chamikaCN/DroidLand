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

    [Range(0.0f, 10.0f)]
    public float maxZoom, minZoom, zoomSpeed, yawSpeed, updownSpeed;
    
    float currrentZoom = 10f, currentYaw = 1f, pitch = 2f, currentUpdown = 1f;

    void Update()
    {
        //currrentZoom -= Input.GetAxis("Mouse ScrollWheel")*zoomSpeed;
        //currrentZoom = Mathf.Clamp(currrentZoom, minZoom, maxZoom);

        currentUpdown -= Input.GetAxis("Vertical") * updownSpeed * Time.deltaTime;
        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log(currentUpdown);
        }
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
}
