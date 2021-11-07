using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;
    public PlayerSlideState SlideState = new PlayerSlideState();
    public PlayerRunState RunState = new PlayerRunState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerWallRunState WallRunState = new PlayerWallRunState();

    public Animator animator;
    public CharacterController controller;
    private PlayerInputs inputs;

    public float VelocityX = 0;
    public float VelocityY = 0;
    public float VelocityZ = 0;
    private int VelocityXHash;
    private int VelocityYHash;
    private int VelocityZHash;


    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        controller = this.GetComponent<CharacterController>();
        inputs = new PlayerInputs();

        VelocityXHash = Animator.StringToHash("VelocityX");
        VelocityYHash = Animator.StringToHash("VelocityY");
        VelocityZHash = Animator.StringToHash("VelocityZ");


        inputs.CharacterControls.Movement.started += onMovement;
        inputs.CharacterControls.Movement.canceled += onMovement;
        inputs.CharacterControls.Movement.performed += onMovement;
        inputs.CharacterControls.Jump.started += onJump;
        inputs.CharacterControls.Jump.canceled += onJump;
        inputs.CharacterControls.Crouch.started += onCrouch;
        inputs.CharacterControls.Crouch.canceled += onCrouch;
        inputs.CharacterControls.MouseX.started += onMouseMove;
        inputs.CharacterControls.MouseX.canceled += onMouseMove;
        inputs.CharacterControls.MouseX.performed += onMouseMove;

    }

    void Start()
    {
        currentState = RunState;

        RunState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
        currentState.UpdateMouse(this);
        animator.SetFloat(VelocityXHash, VelocityX);
        animator.SetFloat(VelocityYHash, VelocityY);
        animator.SetFloat(VelocityZHash, VelocityZ);

    }

    void FixedUpdate()
    {
        controller.Move(transform.TransformDirection(VelocityX, VelocityY, VelocityZ) * Time.deltaTime);

    }

    public void SwitchState(PlayerBaseState state)
    {
        state.currentInputMovement = currentState.currentInputMovement;
        currentState.LeaveState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    public void onMovement(InputAction.CallbackContext context)
    {
        currentState.onMovement(this, context.ReadValue<Vector2>());
    }

    public void onJump(InputAction.CallbackContext context)
    {
        currentState.onJump(this, context.ReadValueAsButton());
    }

    private void onCrouch(InputAction.CallbackContext context)
    {
        currentState.onCrouch(this, context.ReadValueAsButton());
    }

    public void onDrop()
    {
        currentState.onDrop(this);
    }

    public void onMouseMove(InputAction.CallbackContext context)
    {
        currentState.onMouseMove(context.ReadValue<float>());
    }

    private void OnEnable()
    {
        inputs.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        inputs.CharacterControls.Disable();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.normal.y < 0.1f)
        {
            currentState.OnControllerColliderHit(this, hit);
        }
    }
}
