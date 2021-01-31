using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractableButtons : MonoBehaviour
{
    public UIEnum uiType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();

        if (player)
            ShowUI(player);
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponentInParent<Player>();

        if (player)
            HideUI(player);
    }

    public void ShowUI(Player player)
    {
        if (uiType == UIEnum.Jump)
            player.view.ShowJumpUI();
        else
            player.view.ShowMoveUI();
    }

    public void HideUI(Player player)
    {
        if (uiType == UIEnum.Jump)
            player.view.HideJumpUI();
        else
            player.view.HideMoveUI();
    }
}

public enum UIEnum
{
    Jump,
    Move,
}
