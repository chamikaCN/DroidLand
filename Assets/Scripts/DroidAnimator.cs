using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroidAnimator : MonoBehaviour
{
    Animator playerAnim;
    NavMeshAgent agent;
    Droid droid;
    void Start()
    {
        playerAnim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        droid = GetComponent<Droid>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed;
        speed = droid.getMoveAnimValue();
        playerAnim.SetFloat("Move", speed);
    }

    public void ShootAnim(){
        playerAnim.SetTrigger("Shoot");
    }
}
