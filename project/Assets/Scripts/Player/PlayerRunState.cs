using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetBool("isRunning", true);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        handleCharacterMovement(player);

        // gravity
        if (player.controller.isGrounded)
        {
            player.VelocityY = -.1f;
        }
        else
        {
            if (player.VelocityY < maxVelocityY)
            {
                player.VelocityY = maxVelocityY;
                player.animator.SetBool("isFalling", true);
            }
            if (player.VelocityY > maxVelocityY)
            {
                player.animator.SetBool("isFalling", false);
                player.VelocityY -= gravity * Time.deltaTime;
            }
        }
    }

    public override void LeaveState(PlayerStateManager player)
    {
        player.animator.SetBool("isRunning", false);
    }

    public override void onJump(PlayerStateManager player, bool pressed)
    {
        if (player.controller.isGrounded && pressed)
            player.SwitchState(player.JumpState);
    }
    public override void onCrouch(PlayerStateManager player, bool pressed)
    {
        if ((player.VelocityZ == maxVelocityZ || Mathf.Abs(player.VelocityX) == maxVelocityX) && pressed)
            player.SwitchState(player.SlideState);
        // else Crouch State
    }
}