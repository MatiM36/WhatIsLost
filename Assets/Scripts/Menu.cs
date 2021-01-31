using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject credits;

    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }
}
