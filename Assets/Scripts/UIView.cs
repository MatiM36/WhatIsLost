using UnityEngine;

public class UIView : MonoBehaviour
{
    public GameObject interactContainer;
    public GameObject moveContainer;
    public GameObject jumpContainer;

    public void ShowInteractUI()
    {
        interactContainer.SetActive(true);
    }

    public void HideInteractUI()
    {
        interactContainer.SetActive(false);
    }

    public void ShowMoveUI()
    {
        moveContainer.SetActive(true);
    }

    public void HideMoveUI()
    {
        moveContainer.SetActive(false);
    }

    public void ShowJumpUI()
    {
        jumpContainer.SetActive(true);
    }

    public void HideJumpUI()
    {
        jumpContainer.SetActive(false);
    }
}