using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Player player;
    public Animator animator;

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


    private void OnAnimatorIK(int layerIndex)
    {
        if ((player.wallDetected || player.movableObjDetected) && !player.ledgeDetected && player.isOnFloor)
        {
            var right = Vector3.Cross(Vector3.up,player.LastDir);
            var handRot = Vector3.Cross(player.LastDir,right);
            var wallPosition = player.wallDetectorTransform.position + player.LastDir * player.handReach;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, wallPosition - right * player.handSeparation);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.LookRotation(handRot, -player.LastDir));

            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKPosition(AvatarIKGoal.RightHand, wallPosition + right * player.handSeparation);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
            animator.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.LookRotation(handRot, -player.LastDir));
        }
    }
}
