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


    void Start()
    {
        currrentZoom = 1.5f;
        currentYaw = 1f;
        pitch = 2f;
        currentUpdown = 1f;
    }

    void LateUpdate()
    {
        // float clampedY = Mathf.Clamp(target.position.y + offset.y * currrentZoom, 1.5f, 6f);
        Vector3 additionVector = target.position + offset * currrentZoom;
        Mathf.Clamp(additionVector.y, 1.5f, 6f);
        transform.position = additionVector;
        transform.LookAt(target.position + Vector3.up * pitch);

        transform.RotateAround(target.position, Vector3.up, currentYaw);
        transform.RotateAround(target.position, transform.right, currentUpdown);
    }

    public void setTransform(Transform transform)
    {
        target = transform;

    }

    public void CalculateCameraMovement(float horizontal, float vertical)
    {
        currentYaw -= horizontal * yawSpeed * Time.deltaTime;
        currentUpdown -= vertical * updownSpeed * Time.deltaTime;
    }

    public void CalculateCameraZoom(float requestZoom)
    {
        currrentZoom = requestZoom + 1f;
    }

}
