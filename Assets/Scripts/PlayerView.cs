using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Player player;

    public void OnJumpStart()
    {
    }

    public void OnClimbEnd()
    {
        player.OnClimbEnd();
        transform.localPosition = Vector3.zero;
    }
}
