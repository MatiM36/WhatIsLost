using Mati36.Vinyl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dummy : MonoBehaviour
{
    public VinylAsset exampleSound;

    public VinylAsset bg1, bg2;

    private VinylAudioSource currentMusic, currentFx;

    [ReadOnly]
    public bool isNull, isPlaying;

    private void Start()
    {
        if (bg1 != null)
        {
            currentMusic = bg1.Play();
            //currentMusic = VinylManager.PlaySound(bg1);
        }
    }

    private void Update()
    {
        isNull = currentFx == null;
        if (currentFx != null)
            isPlaying = currentFx.IsPlaying;

        if (Input.GetKeyDown(KeyCode.P))
        {
            //VinylManager.PlaySound(exampleSound);
            if (currentFx == null)
            {
                currentFx = exampleSound.Play();
                currentFx.Lock();
                currentFx.e_OnEndSound += (src) => Debug.Log("END SOUND");
            }
            else
                currentFx.RestartSource();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (bg1 != null)
            {
                //currentMusic = bg1.Play();
                if (currentMusic.CurrentAsset == bg1)
                    currentMusic = currentMusic.CrossFadeTo(bg2, 1);
                else
                    currentMusic = currentMusic.CrossFadeTo(bg1, 1);
                //currentMusic = VinylManager.PlaySound(bg1);
            }
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    currentMusic.FadeTo(1, 0.4f);
        //}
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    currentMusic.FadeTo(0.5f, 0.4f);
        //}
        ////VinylManager.CrossFadeTo(ref currentMusic, bg2, 1);

        //if (Input.GetKeyDown(KeyCode.Keypad1))
        //    SceneManager.LoadScene(0);
        //if (Input.GetKeyDown(KeyCode.Keypad2))
        //    SceneManager.LoadScene(1);
    }
}
