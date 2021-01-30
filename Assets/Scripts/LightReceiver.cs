using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightReceiver : MonoBehaviour, IActivator
{
    public Transform lightCenter;
    public Activatable activable;

    public float activationDuration = 0.5f;

    private float activationTimer;

    public bool activated = false;

    public void Toggle(bool state)
    {
        if (!activated)
        {
            activated = true;
            activable.Toggle(activated);
        }

        activationTimer = activationDuration;
    }


    private void Update()
    {
        if(activated && activationTimer > 0)
        {
            activationTimer -= Time.deltaTime;
            if(activationTimer <= 0)
            {
                activated = false;
                activable.Toggle(activated);
            }
        }
    }
}
