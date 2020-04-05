using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Droid : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject[] goals;
    GameObject currentGoal;
    public GameObject ball;
    bool playerControlled, attacked;
    int enemiesInRange;
    List<Transform> detectedEnemies;
    string Team;
    Vector3 movingDirection;
    Transform currentEnemyTransform;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        Team = this.gameObject.tag.ToString();
        playerControlled = false;
        enemiesInRange = 0;
        currentEnemyTransform = null;
        attacked = false;
        detectedEnemies = new List<Transform>();

        goals = GameObject.FindGameObjectsWithTag("Goal");
        RoamToNext();
    }

    void Update()
    {
        if (!playerControlled)
        {
            if (enemiesInRange == 0)
            {
                Roam();
            }
            else
            {
                Transform enemyTransform = detectedEnemies[0];
                FollowEnemy(enemyTransform);
            }
        }
    }

    public void setPlayerControl()
    {
        playerControlled = true;
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.isStopped = true;
    }

    public void ResetPlayerControl()
    {
        agent.isStopped = false;
        RoamToNext();
        playerControlled = false;
    }

    void Roam()
    {
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


    public void ManualMovement(Vector3 target)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
        agent.stoppingDistance = 0f;
    }

    Vector3 getMovingDirection()
    {
        Vector3 dir = GetComponent<Rigidbody>().velocity.normalized;
        if (dir.magnitude == 0)
        {
            dir = Vector3.forward;
        }
        return dir;

    }

    public Vector3 getAttackDirection()
    {
        return movingDirection;
    }

    public string getTeam()
    {
        return Team;
    }

    public void EnemyEnterDetection(Collider other)
    {
        if (!playerControlled)
        {
            if (other.name == "Droid" && other.tag != Team)
            {
                if (enemiesInRange == 0)
                {
                    currentEnemyTransform = other.gameObject.transform;
                }
                enemiesInRange += 1;
                detectedEnemies.Add(other.gameObject.transform);
            }
        }

    }

    public void EnemyExitDetection(Collider other)
    {
        if (!playerControlled)
        {
            if (other.name == "Droid" && other.tag != Team)
            {
                enemiesInRange -= 1;
                detectedEnemies.Remove(other.gameObject.transform);
                if (enemiesInRange > 0)
                {
                    currentEnemyTransform = detectedEnemies[0];
                }
                else
                {
                    RoamToNext();
                }
            }
        }
    }

    void FollowEnemy(Transform enemy)
    {

    }

    void OnDrawGizmosSelected()
    {
        if (tag == "Orange")
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawWireSphere(transform.position, 1.5f);
    }

    public void Attack(Vector3 unitVector)
    {
        if (!attacked)
        {
            movingDirection = unitVector;
            Debug.Log(GetComponent<Rigidbody>().velocity);
            Vector3 position = new Vector3(transform.position.x, 0.25f, transform.position.z);
            Instantiate(ball, position, Quaternion.identity, this.transform);
            attacked = true;
            StartCoroutine(AttackReset());
        }
    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(2f);
        attacked = false;
    }

}

