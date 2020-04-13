using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager instance;
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

    public GameObject gamePanel, pausePanel;
    public Button blastButton, guardButton;
    public Slider slider;
    public Joystick moveJoystick, cameraJoystick;

    void Start()
    {
        pausePanel.SetActive(false);
        gamePanel.SetActive(true);
        slider.value = 0.4f;
    }

    void Update()
    {
        CameraJoystickUpdate();
        MoveJoystickUpdate();
    }

    public void SliderValueUpdate()
    {
        CameraController.instance.CalculateCameraZoom(slider.value);
    }

    void CameraJoystickUpdate()
    {
        float hor = cameraJoystick.Horizontal, ver = cameraJoystick.Vertical;
        if (Mathf.Abs(ver * hor) > 0)
        {
            CameraController.instance.CalculateCameraMovement(hor, ver);
        }
    }

    void MoveJoystickUpdate()
    {
        Vector3 move = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);
        PlayerManager.instance.PlayerMovement(move);
    }

    public void AttackButtonUpdate()
    {
        PlayerManager.instance.PlayerAttack();
    }

    public void ChangeButtonUpdate()
    {
        PlayerManager.instance.PlayerChange();
    }

    public void BlastButtonUpdate()
    {
        bool blasted = PlayerManager.instance.PlayerBlast();
        if (blasted)
        {
            blastButton.enabled = false;
            StartCoroutine(blastButtonReset());
        }
    }

    public void GuardButtonUpdate()
    {
        bool guardOn = PlayerManager.instance.PlayerGuard();
        if (guardOn)
        {
            guardButton.enabled = false;
            StartCoroutine(guardButtonReset());
        }
    }

    IEnumerator blastButtonReset()
    {
        yield return new WaitForSeconds(8);
        blastButton.enabled = true;
    }

    IEnumerator guardButtonReset()
    {
        yield return new WaitForSeconds(15);
        guardButton.enabled = true;
    }

}
