using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    Droid droid;

    private void Start() {
        droid = GetComponentInParent<Droid>();
    }
    private void OnTriggerEnter(Collider other) {
        droid.EnemyEnterDetection(other);
    }

    private void OnTriggerExit(Collider other) {
        droid.EnemyExitDetection(other);
    }
}
