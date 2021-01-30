using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Player player;

    public void OnJumpStart()
    {
        player.OnJumpStart();
    }

    

    public void OnJumpEnd()
    {
        player.OnJumpEnd();
    }

    public void OnClimbStart()
    {
        player.OnClimbStart();
    }

    public void OnClimbEnd()
    {
        player.OnClimbEnd();
    }
}
