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
    int enemiesInRange, health;
    List<Transform> detectedEnemies;
    string Team;
    Vector3 shootDirection;
    Transform currentEnemyTransform;
    DroidAnimator animator;
    float playerControlSpeed;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<DroidAnimator>();

        Team = this.gameObject.tag.ToString();
        //playerControlled = false;
        enemiesInRange = 0;
        health = 3;
        currentEnemyTransform = null;
        attacked = false;
        playerControlSpeed = 0f;
        detectedEnemies = new List<Transform>();

        goals = GameObject.FindGameObjectsWithTag("Goal");
        RoamToNext();
    }

    void Update()
    {
        if (!playerControlled)
        {
            if (enemiesInRange <= 0)
            {
                Roam();
            }
            else
            {
                Transform enemyTransform = detectedEnemies[0];
                AttackEnemyAuto(enemyTransform);

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

    public Vector3 getMovingDirection()
    {
        Vector3 dir;
        if (playerControlled)
        {
            dir = GetComponent<Rigidbody>().velocity.normalized;
            if (dir.magnitude == 0)
            {
                dir = Vector3.forward;
            }
        }
        else
        {
            dir = agent.velocity;
        }
        return dir;
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

    public void RemoveEnemy(Droid enemyDroid)
    {
        if (currentEnemyTransform == enemyDroid.gameObject.transform)
        {
            if (enemiesInRange > 1)
            {
                currentEnemyTransform = detectedEnemies[1];
                enemiesInRange -= 1;

            }
            else if (enemiesInRange == 1)
            {
                RoamToNext();
                enemiesInRange -= 1;

            }
            detectedEnemies.Remove(enemyDroid.gameObject.transform);
        }
        else if (detectedEnemies.Contains(enemyDroid.gameObject.transform))
        {
            enemiesInRange -= 1;
            detectedEnemies.Remove(enemyDroid.gameObject.transform);
        }
    }

    void AttackEnemyAuto(Transform enemy)
    {
        Vector3 direction = new Vector3(enemy.transform.position.x - transform.position.x,
        enemy.transform.position.y - transform.position.y, enemy.transform.position.z - transform.position.z);
        Attack(direction);
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

    public void Attack(Vector3 directionVector)
    {
        if (!attacked)
        {
            shootDirection = directionVector.normalized;
            animator.ShootAnim();
            // if(playerControlled){
            //     agent.
            // }
            Vector3 position = new Vector3(transform.position.x, 0.25f, transform.position.z);
            GameObject fireball =  Instantiate(ball, position, Quaternion.identity);
            FireBall fb = fireball.GetComponent<FireBall>();
            fb.setDirection(shootDirection);
            fb.setTeam(Team);
            attacked = true;
            StartCoroutine(AttackReset());
        }
    }

    public void getDamage()
    {

        health = health - 1;
        if (health == 1)
        {
            this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else if (health == 0)
        {
            GameSceneManager.instance.DestroyCheck(this);
            this.gameObject.SetActive(false);
        }

    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(2f);
        attacked = false;
    }

    public float getMoveAnimValue(){

        float speed = 0f;
        if (!playerControlled)
        {
            speed = agent.velocity.magnitude / agent.speed;
        }else{
            speed = playerControlSpeed;
        }
        return speed;
    }

    public void setPlayerControlSpeed(float sp){
        playerControlSpeed = sp;
    }
}

