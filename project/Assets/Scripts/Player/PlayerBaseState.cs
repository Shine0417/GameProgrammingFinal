using UnityEngine;

public abstract class PlayerBaseState
{
    protected float accelerateX = 12f;
    protected float accelerateZ = 12f;
    protected float maxVelocityX = 8f;
    protected float maxVelocityZ = 8f;
    protected float maxVelocityY = -8f;
    protected float oppositeAccelerateScale = 3;
    protected float gravity = 9.8f;
    public Vector2 currentInputMovement;
    protected float mouseInputX;

    protected bool closeToGround = true;
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void LeaveState(PlayerStateManager player);
    public void onMovement(PlayerStateManager player, Vector2 movement)
    {
        currentInputMovement = movement;
    }
    public abstract void onJump(PlayerStateManager player, bool pressed);
    public abstract void onCrouch(PlayerStateManager player, bool pressed);

    public virtual void OnControllerColliderHit(PlayerStateManager player, ControllerColliderHit hit)
    {
    }

    public void onDrop(PlayerStateManager player)
    {
        closeToGround = true;
        player.animator.SetBool("isFalling", false);
    }

    public virtual void onMouseMove(float mouseX)
    {
        mouseInputX = mouseX;
    }

    public virtual void UpdateMouse(PlayerStateManager player) {
        player.gameObject.transform.Rotate(Vector3.up, mouseInputX * 50 * Time.deltaTime);
    }

    protected void handleCharacterMovement(PlayerStateManager player)
    {
        // run left & right
        if (currentInputMovement.x != 0)
        {
            handleVelocityXaccelerate(player);
        }
        else
        {
            handleVelocityXDecelerate(player);
        }

        // run forward & backward
        if (currentInputMovement.y != 0)
        {
            handleVelocityZaccelerate(player);
        }
        else
        {
            handleVelocityZDecelerate(player);
        }
    }

    protected void handleVelocityXDecelerate(PlayerStateManager player)
    {
        if (player.VelocityX > 0)
        {
            player.VelocityX -= oppositeAccelerateScale * accelerateX * Time.deltaTime;
            if (player.VelocityX < 0) player.VelocityX = 0;
        }
        else if (player.VelocityX < 0)
        {
            player.VelocityX += oppositeAccelerateScale * accelerateX * Time.deltaTime;
            if (player.VelocityX > 0) player.VelocityX = 0;
        }
    }

    protected void handleVelocityZDecelerate(PlayerStateManager player)
    {

        if (player.VelocityZ > 0)
        {
            player.VelocityZ -= oppositeAccelerateScale * accelerateZ * Time.deltaTime;
            if (player.VelocityZ < 0) player.VelocityZ = 0;
        }
        else if (player.VelocityZ < 0)
        {
            player.VelocityZ += oppositeAccelerateScale * accelerateZ * Time.deltaTime;
            if (player.VelocityZ > 0) player.VelocityZ = 0;
        }
    }

    protected void handleVelocityXaccelerate(PlayerStateManager player)
    {
        player.VelocityX += currentInputMovement.x * accelerateX * Time.deltaTime;
        if (player.VelocityX > maxVelocityX) player.VelocityX = maxVelocityX;
        if (player.VelocityX < -maxVelocityX) player.VelocityX = -maxVelocityX;
        if (player.VelocityX * currentInputMovement.x < 0)
        {
            player.VelocityX += oppositeAccelerateScale * currentInputMovement.x * accelerateX * Time.deltaTime;
        }
    }
    protected void handleVelocityZaccelerate(PlayerStateManager player)
    {
        player.VelocityZ += currentInputMovement.y * accelerateZ * Time.deltaTime;
        if (player.VelocityZ > maxVelocityZ) player.VelocityZ = maxVelocityZ;
        if (player.VelocityZ < -maxVelocityZ) player.VelocityZ = -maxVelocityZ;
        if (player.VelocityZ * currentInputMovement.y < 0)
            player.VelocityZ += oppositeAccelerateScale * currentInputMovement.y * accelerateZ * Time.deltaTime;
    }
}