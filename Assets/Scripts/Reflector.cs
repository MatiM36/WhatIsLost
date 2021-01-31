using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflector : MonoBehaviour, IActivatable
{
    public Transform forwardTransform;
    
    public ParticleSystemRenderer lightRenderer;
    
    public float activationDuration = 0.2f;

    private float activationTimer;

    public bool activated = false;

    private void Awake()
    {
        lightRenderer.enabled = false;
    }

    public void Toggle(bool state)
    {
        activationTimer = activationDuration;
        activated = true;
        lightRenderer.enabled = true;
    }

    private void Update()
    {
        if(activated && activationDuration > 0)
        {
            activationTimer -= Time.deltaTime;
            if (activationTimer <= 0)
            {
                activated = false;
                lightRenderer.enabled = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(forwardTransform != null)
        Gizmos.DrawLine(forwardTransform.position, forwardTransform.position + forwardTransform.forward);
    }
}
