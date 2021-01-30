using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    public string levelName;

    public void GoToNextLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();

        if (player)
            GoToNextLevel();
    }
}
