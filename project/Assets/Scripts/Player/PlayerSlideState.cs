using UnityEngine;

public class PlayerSlideState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetBool("isSliding", true);
        oppositeAccelerateScale = 1f;
        maxVelocityZ = maxVelocityZ * 2;
        player.VelocityZ = player.VelocityZ * 2;
        maxVelocityX = maxVelocityX * 2;
        player.VelocityX = player.VelocityX * 2;

        Vector3 newCenter = player.controller.center;
        newCenter.y = newCenter.y - player.controller.height/4;
        player.controller.center = newCenter;
        player.controller.height /= 2;
    }
    public override void UpdateState(PlayerStateManager player)
    {
        handleVelocityXDecelerate(player);
        handleVelocityZDecelerate(player);

        if (player.VelocityX == 0 && player.VelocityZ == 0)
        {
            player.SwitchState(player.RunState);
        }
        if (player.controller.isGrounded)
        {
            player.VelocityY = -.1f;
        }
        else
        {
            player.VelocityY -= gravity * Time.deltaTime;
            if (player.VelocityY == maxVelocityY) player.VelocityY = maxVelocityY;
        }
    }
    public override void LeaveState(PlayerStateManager player)
    {
        player.animator.SetBool("isSliding", false);
        
        Vector3 newCenter = player.controller.center;
        newCenter.y = newCenter.y + player.controller.height/2;
        player.controller.center = newCenter;
        player.controller.height *= 2;
    }
    public override void onJump(PlayerStateManager player, bool pressed)
    {
        if (player.controller.isGrounded && pressed)
        {
            player.SwitchState(player.JumpState);
        }
    }
    public override void onCrouch(PlayerStateManager player, bool pressed)
    {
        if (!pressed)
            player.SwitchState(player.RunState);
    }
}