using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float JumpVeclocity = 5f;

    private bool crouchPressed = false;
    public override void EnterState(PlayerStateManager player)
    {
        player.animator.SetBool("isJumping", true);
        float speedScale = 1 + 0.5f * Mathf.Max(player.VelocityX / maxVelocityX, player.VelocityZ / maxVelocityZ);
        player.VelocityY = JumpVeclocity * speedScale;
    }
    public override void UpdateState(PlayerStateManager player)
    {
        handleCharacterMovement(player);

        player.VelocityY -= gravity * Time.deltaTime;
        if (player.VelocityY == maxVelocityY) player.VelocityY = maxVelocityY;

        if (player.VelocityY < 0 && player.controller.isGrounded)
        {
            player.VelocityY = 0;
            if (crouchPressed)
                player.SwitchState(player.SlideState);
            else
                player.SwitchState(player.RunState);
        }
    }

    public override void LeaveState(PlayerStateManager player)
    {
        player.animator.SetBool("isJumping", false);
    }
    public override void onJump(PlayerStateManager player, bool pressed)
    {
    }
    public override void onCrouch(PlayerStateManager player, bool pressed)
    {
        crouchPressed = pressed;
    }

    public override void OnControllerColliderHit(PlayerStateManager player, ControllerColliderHit hit)
    {
        Vector3 direction = player.transform.TransformDirection(Vector3.forward);
        // if (Mathf.Abs(hit.normal.x * direction.x + hit.normal.z * direction.z) < 0.7)
        // {
            player.WallRunState.WallNormal = hit.normal;
            player.SwitchState(player.WallRunState);
        // }
    }
}