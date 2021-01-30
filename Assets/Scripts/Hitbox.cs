using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public event Action e_OnHit;

    private void OnTriggerEnter(Collider other)
    {
        var hurtbox = other.GetComponent<Hurtbox>();
        if (hurtbox != null)
        {
            hurtbox.ReceiveDamage();
            e_OnHit?.Invoke();
        }
    }
}
