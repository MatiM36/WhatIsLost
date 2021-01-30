using UnityEngine;

public class UIView : MonoBehaviour
{
    public GameObject interactContainer;

    public void ShowInteractUI()
    {
        interactContainer.SetActive(true);
    }

    public void HideInteractUI()
    {
        interactContainer.SetActive(false);
    }
}