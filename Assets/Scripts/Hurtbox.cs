using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public event Action e_OnHitReceived;

    public void ReceiveDamage()
    {
        e_OnHitReceived?.Invoke();
    }
}
