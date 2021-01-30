using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractuable
{
    public event Action e_OnInteractionReceived;

    public void Interact()
    {
        e_OnInteractionReceived?.Invoke();
    }
}
