using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Droid : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject[] goals;
    GameObject currentGoal;
    bool playerControlled;
    //Vector3 movingDirection;
    //public GameObject ball;
    //public LayerMask creature;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        playerControlled = false;
        goals = GameObject.FindGameObjectsWithTag("Goal");
        RoamToNext();

    }

    void Update()
    {
        if (!playerControlled)
        {
            Roam();
        }
    }

    public void setPlayerControl(){
        playerControlled = true;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.isStopped = true;
    }

    public void ResetPlayerControl(){
        agent.isStopped = false;
        RoamToNext();
        playerControlled = false;
    }

    void Roam(){
        if (Vector3.Distance(transform.position, currentGoal.transform.position) < 1)
        {
            RoamToNext();
        }
    }

    void RoamToNext()
    {
        currentGoal = goals[Random.Range(0, goals.Length)];
        agent.SetDestination(currentGoal.transform.position);
    }

    // public void ChangeTest(){
    //     Debug.Log("changed to " + transform.name);
    // }

    public void ManualMovement(Vector3 target){
        agent.isStopped = false;
        agent.SetDestination(target);
        agent.stoppingDistance = 0f;
    }

    // public void Attack(){
    //     Debug.Log("Attacked");
    //     Instantiate(ball, new Vector3(transform.position.x,transform.position.y+1,
    //         transform.position.z), Quaternion.identity, this.transform);
    // }

    // void setMovingDirection(Vector3 direction){
    //     if(movingDirection != direction){
    //         movingDirection = direction;
    //     }
    // }

    // public Vector3 getMovingDirection(){
    //     return movingDirection;
    // }

    // public void RangeCheck(){
    //     RaycastHit hit;

    //     if(Physics.Raycast(transform.position,new Vector3(1,0,0),50f,creature)){
    //         Debug.DrawRay(transform.position, new Vector3(1, 0, 0), Color.blue, 4f);
    //         Debug.Log("hit");
    //     }
    // }

    
}
