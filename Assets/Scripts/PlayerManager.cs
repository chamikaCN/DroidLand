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
    public Joystick moveJoystick;
    public Button blastButton, guardButton;
    Rigidbody player;
    float speed;
    Vector3 faceDirection;
    void Start()
    {
        cam = Camera.main;
        faceDirection = Vector3.forward;
        speed = 75f;
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

    public void PlayerMovement(Vector3 movement)
    {
        movement = cam.transform.TransformDirection(movement);
        Vector3 moveDirection = movement;
        moveDirection.y = 0;
        moveDirection = moveDirection.normalized * movement.magnitude;
        player.velocity = moveDirection * Time.deltaTime * speed;
        if (player.velocity.magnitude > 0)
        {
            faceDirection = player.velocity;
        }
        player.transform.rotation = Quaternion.LookRotation(faceDirection, transform.up);
        currentDroid.setPlayerControlSpeed(player.velocity.magnitude);
    }


    public void PlayerAttack()
    {
        currentDroid.Attack(currentDroid.getMovingDirection());
    }

    public void PlayerChange()
    {
        GameSceneManager.instance.ChangeDroid();
    }

    public bool PlayerBlast(){
        return currentDroid.Blast();
    }

    public bool PlayerGuard(){
        return currentDroid.ActivateGuard();
    }

    public void SpeedupDroid(){
        speed = 120f;
    }

    
}

