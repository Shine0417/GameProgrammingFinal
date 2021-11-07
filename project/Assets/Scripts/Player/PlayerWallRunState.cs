using UnityEngine;

public class PlayerWallRunState : PlayerBaseState
{
    public Vector3 WallNormal;
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetBool("isWallRunning", true);
        Vector3 direction = player.transform.TransformDirection(Vector3.forward);
        int left = (WallNormal.x * direction.z + WallNormal.z * direction.x) > 0 ? -1 : 1;
        player.gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(WallNormal.z, 0, -WallNormal.x) * left, Vector3.up);

        player.VelocityY = 0;
        player.VelocityX = 0;
        player.animator.SetFloat("isWallRight", left);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        player.VelocityZ -= 0.03f * oppositeAccelerateScale * accelerateX * Time.deltaTime;
        if (player.VelocityZ < maxVelocityZ / 3)
        {
            player.SwitchState(player.RunState);
        }
    }

    public override void LeaveState(PlayerStateManager player)
    {
        player.animator.SetBool("isWallRunning", false);
    }
    public override void onJump(PlayerStateManager player, bool pressed)
    {
        player.animator.SetBool("isJumping", true);
        player.SwitchState(player.JumpState);
    }
    public override void onCrouch(PlayerStateManager player, bool pressed)
    {
    }

    public override void onMouseMove(float mouseX)
    {
        
    }
}