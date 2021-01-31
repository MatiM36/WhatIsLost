using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotoSlider : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public RawImage image;
    public RawImage nextImage;

    public Button nextButton;
    public float transitionTime = 1f;
    public List<Texture2D> imagesToShow = new List<Texture2D>();
    private int currentImage = 0;
    private Coroutine currentShowRoutine = null;

    public string nextScene = "";

    private void Start()
    {
        nextImage.color = Color.clear;
        image.color = Color.white;
        image.texture = imagesToShow[0];
        currentImage = 1;
        nextButton.gameObject.SetActive(true);
    }

    public void NextPhoto()
    {
        if (currentImage >= imagesToShow.Count) return;
        if (currentShowRoutine == null)
            currentShowRoutine = StartCoroutine(ShowRoutine(imagesToShow[currentImage]));
    }

    IEnumerator ShowRoutine(Texture2D photo)
    {
        nextButton.gameObject.SetActive(false);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / transitionTime;

            image.color = new Color(1, 1, 1, 1 - t);
            yield return null;
        }

        currentImage++;
        if (currentImage >= imagesToShow.Count)
        {
            sceneLoader.LoadSceneWithFade(nextScene);
        }
        else
        {
            nextImage.texture = photo;
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * transitionTime;

                nextImage.color = new Color(1, 1, 1, t);
                yield return null;
            }

            nextImage.color = Color.clear;
            image.color = Color.white;
            image.texture = photo;

            nextButton.gameObject.SetActive(true);
        }

        currentShowRoutine = null;

    }
}
