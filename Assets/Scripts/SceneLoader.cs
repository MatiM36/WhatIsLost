using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    static public SceneLoader Instance { get; private set; }

    public Image blackImage;

    private Coroutine fadeRoutine = null;

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        fadeRoutine = StartCoroutine(FadeInRoutine(1f));
    }

    public void LoadSceneWithFade(string name)
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOutRoutine(name, 1f));
    }

    private IEnumerator FadeInRoutine(float fadeDuration)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, 1 - t);
            yield return null;
        }
        blackImage.gameObject.SetActive(false);
        fadeRoutine = null;
    }

    private IEnumerator FadeOutRoutine(string name, float fadeDuration)
    {
        blackImage.gameObject.SetActive(true);

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            blackImage.color = new Color(0, 0, 0, t);
            yield return null;
        }
        fadeRoutine = null;
        LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
