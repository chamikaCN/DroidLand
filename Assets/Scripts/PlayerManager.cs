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
    float sliderValue;
    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        PlayerMovement();
        CameraMovement();
        PlayerAttack();
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
        Vector3 movement = new Vector3(-moveJoystick.Vertical, 0, moveJoystick.Horizontal);
        player.velocity = movement * Time.deltaTime * 50f;
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

    void PlayerAttack(){
        if(Input.GetKey(KeyCode.Space)){
            currentDroid.Attack(currentDroid.getAttackDirection());
        }
    }
}

