using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    Vector3 direction;
    Droid parent;
    string team;
    [Range(0f, 10f)]
    public float speed;
    void Start()
    {
        parent = GetComponentInParent<Droid>();
        direction = parent.getAttackDirection();
        team = parent.getTeam();
    }

    
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
