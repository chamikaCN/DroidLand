using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Droid : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject[] goals;
    GameObject currentGoal, guard;
    public GameObject ball, bomb;
    bool playerControlled, attacked, blasted, guarded, guardUsed;
    int enemiesInRange, health, blastTimer, maxHealth;
    List<Transform> detectedEnemies;
    string Team;
    Vector3 shootDirection;
    Transform currentEnemyTransform;
    DroidAnimator animator;
    float playerControlSpeed;
    HealthBar healthBar;
    Canvas healthCanvas;

    void Awake() {
        healthCanvas = GetComponentInChildren<Canvas>();
        healthBar = healthCanvas.gameObject.GetComponentInChildren<HealthBar>();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
       
        animator = GetComponent<DroidAnimator>();
        guard = GetComponentInChildren<ProtectionGuard>().gameObject;
        guard.SetActive(false);
        
        

        Team = this.gameObject.tag.ToString();
        //playerControlled = false;
        enemiesInRange = 0;
        blastTimer = 0;
        maxHealth = 5;
        health = 5;
        currentEnemyTransform = null;
        attacked = false;
        blasted = false;
        guarded = false;
        guardUsed = false;
        playerControlSpeed = 0f;
        detectedEnemies = new List<Transform>();
        healthBar.setMaxHealth(maxHealth);

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
                if (!blasted)
                {
                    BlastEnemyAuto();
                }
            }
            if (!guardUsed)
            {
                GuardEnemyAuto();
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

    void BlastEnemyAuto()
    {
        if (blastTimer < 2)
        {
            if (enemiesInRange > 2)
            {
                Blast();
            }
            else
            {
                StartCoroutine(BlastCheckTimer());
            }
        }
        else if (blastTimer < 4)
        {
            if (enemiesInRange > 1)
            {
                Blast();
            }
            else
            {
                StartCoroutine(BlastCheckTimer());
            }
        }
        else
        {
            if (enemiesInRange > 0)
            {
                Blast();
            }
        }
    }

    void GuardEnemyAuto()
    {
        if (health == 1)
        {
            ActivateGuard();
        }
        else if (health == 2 && enemiesInRange > 0)
        {
            ActivateGuard();
        }
        else if (enemiesInRange > 1)
        {
            ActivateGuard();
        }

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
            Vector3 position = new Vector3(transform.position.x, 0.25f, transform.position.z);
            GameObject fireball = Instantiate(ball, position, Quaternion.identity);
            FireBall fb = fireball.GetComponent<FireBall>();
            fb.setDirection(shootDirection);
            fb.setTeam(Team);
            attacked = true;
            StartCoroutine(AttackReset());
        }
    }

    public bool Blast()
    {
        if (!blasted)
        {
            Instantiate(bomb, transform.position, transform.rotation);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1);
            foreach (Collider col in colliders)
            {
                if (col.name == "Droid" && col.tag != Team)
                {
                    col.GetComponent<Droid>().getDamage();
                }
            }
            blastTimer = 0;
            blasted = true;
            StartCoroutine(BlastReset());
            return true;
        }
        return false;
    }

    public bool ActivateGuard()
    {
        if (!guardUsed)
        {
            guard.SetActive(true);
            guarded = true;
            guardUsed = true;
            StartCoroutine(GuardReset());
            StartCoroutine(GuardTimer());
            return true;
        }
        return false;
    }

    public void getDamage()
    {
        if (!guarded)
        {
            health = health - 1;
            healthBar.changeHealth(health);
            if (health == 1)
            {
                this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                agent.speed = 1.2f;
            }
            else if (health == 0)
            {
                GameSceneManager.instance.DestroyCheck(this);
                this.gameObject.SetActive(false);
            }
        }
    }

    public float getMoveAnimValue()
    {
        float speed = 0f;
        if (!playerControlled)
        {
            speed = agent.velocity.magnitude / agent.speed;
        }
        else
        {
            speed = playerControlSpeed;
        }
        return speed;
    }

    public void setPlayerControlSpeed(float sp)
    {
        playerControlSpeed = sp;
    }

    IEnumerator AttackReset()
    {
        yield return new WaitForSeconds(2f);
        attacked = false;
    }

    IEnumerator BlastReset()
    {
        yield return new WaitForSeconds(8f);
        blasted = false;
    }

    IEnumerator BlastCheckTimer()
    {
        yield return new WaitForSeconds(2f);
        blastTimer += 1;
    }

    IEnumerator GuardTimer()
    {
        yield return new WaitForSeconds(2f);
        guarded = false;
        guard.SetActive(false);
    }

    IEnumerator GuardReset()
    {
        yield return new WaitForSeconds(12f);
        guardUsed = false;
    }

    public int getHealth()
    {
        return health;
    }

    public void ActivateHealthBar(){
        healthCanvas.gameObject.SetActive(true);
    }

    public void DeactivateHealthBar(){
        healthCanvas.gameObject.SetActive(false);
    }


}

