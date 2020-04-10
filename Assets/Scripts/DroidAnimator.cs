using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroidAnimator : MonoBehaviour
{
    Animator playerAnim;
    NavMeshAgent agent;
    void Start()
    {
        playerAnim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

        float speed = agent.velocity.magnitude / agent.speed;
        playerAnim.SetFloat("Move", speed);
    }

    public void ShootAnim(){
        playerAnim.SetTrigger("Shoot");
    }
}
