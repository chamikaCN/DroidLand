using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager instance;
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

    Camera cam;
    public LayerMask groundMask;
    Droid currentDroid;
    public Joystick moveJoystick, cameraJoystick;
    public Slider slider;
    Rigidbody player;
    float sliderValue, speed;
    Vector3 faceDirection;
    void Start()
    {
        cam = Camera.main;
        faceDirection = Vector3.forward;
    }

    void Update()
    {
        PlayerMovement();
        CameraMovement();
    }

    public void setCurrentDroid(Droid droid)
    {
        currentDroid = droid;
        player = currentDroid.gameObject.GetComponent<Rigidbody>();
        player.isKinematic = false;
    }

    public void removeCurrentDroid()
    {
        player.isKinematic = true;
    }

    void PlayerMovement()
    {
        Vector3 movement = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);
        movement = cam.transform.TransformDirection(movement);
        Vector3 moveDirection = movement;
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized * movement.magnitude;
        player.velocity = moveDirection * Time.deltaTime * 75f;
        if (player.velocity.magnitude > 0)
        {
            faceDirection = player.velocity;
        }
        player.transform.rotation = Quaternion.LookRotation(faceDirection, transform.up);
        currentDroid.setPlayerControlSpeed(player.velocity.magnitude);
    }

    void CameraMovement()
    {
        sliderValue = 0f;
        float hor = cameraJoystick.Horizontal, ver = cameraJoystick.Vertical;
        if (Mathf.Abs(ver * hor) > 0)
        {
            CameraController.instance.CalculateCameraMovement(hor, ver);
        }

        if (slider.value != sliderValue)
        {
            sliderValue = slider.value;
            CameraController.instance.CalculateCameraZoom(sliderValue);
        }
    }

    public void PlayerAttack()
    {
        currentDroid.Attack(currentDroid.getMovingDirection());
    }

    public void PlayerChange()
    {
        GameSceneManager.instance.ChangeDroid();
    }

}

