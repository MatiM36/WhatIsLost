using Mati36.Vinyl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject credits;

    public VinylAsset mainMenuMusic;

    private void Start()
    {
        mainMenuMusic.Play();
    }
    public void ToggleCredits()
    {
        credits.SetActive(!credits.activeSelf);
    }
}
