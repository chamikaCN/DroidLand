using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    Rigidbody player;

    void Start()
    {
        cam = Camera.main;
        
        //currentDroid = GameSceneManager.instance.getCurrentDroid();
    }

    void Update()
    {
        PlayerMovement();

        
            CameraMovement();

        
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;

        //     if (Physics.Raycast(ray, out hit, 100, groundMask))
        //     {
        //         currentDroid.ManualMovement(hit.point);
        //     }

        // }

        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     currentDroid.Attack();
        // }


    }

    public void setCurrentDroid(Droid droid)
    {
        currentDroid = droid;
        player = currentDroid.gameObject.GetComponent<Rigidbody>();
        player.isKinematic = false;
    }

    public void removeCurrentDroid(){
        player.isKinematic = true;
    }

    void PlayerMovement(){
        Vector3 movement = new Vector3(-moveJoystick.Vertical,0,moveJoystick.Horizontal);
        player.velocity = movement*Time.deltaTime*50f;
    }

    void CameraMovement()
    {
        //take user touch controls and provide movement;
        float hor = cameraJoystick.Horizontal, ver = cameraJoystick.Vertical;
        if (Mathf.Abs(ver * hor) > 0) {
            CameraController.instance.CalculateCameraMovement(hor, ver);
        }
    }
}

