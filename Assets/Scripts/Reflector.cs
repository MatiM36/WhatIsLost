using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    public Transform forwardTransform;

    private void OnDrawGizmos()
    {
        if(forwardTransform != null)
        Gizmos.DrawLine(forwardTransform.position, forwardTransform.position + forwardTransform.forward);
    }
}
