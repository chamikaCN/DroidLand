using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DeleteExplotion());
    }

    IEnumerator DeleteExplotion()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
