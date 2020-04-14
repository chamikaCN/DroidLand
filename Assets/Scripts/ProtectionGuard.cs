using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectionGuard : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(10f, 0, 0)*Time.deltaTime*50f, Space.World);
    }
}
