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
        StartCoroutine(DestroyTime());
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Droid" && other.tag != team)
        {
            other.GetComponent<Droid>().getDamage();
            Destroy(this.gameObject);
        }
        
    }

    public void setDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void setTeam(string name)
    {
        team = name;
    }
}
