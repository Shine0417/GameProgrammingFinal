  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    float rotationfactorPerFrame = 15.0f;
    bool isJumpPressed = false;

    int zero = 0;
    float gravity = -9.8f;
    float groundedGravity = -.05f;
    float initialJumpVelocity;
    float maxJumpHeight;
    float maxJumpTime;

    void Awake(){
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput; // for controller
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        //playerInput.CharacterControls.Run.performed += onRun;
    }
    
    void onMovementInput(InputAction.CallbackContext context){
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x*3.0f;
        currentRunMovement.z = currentMovementInput.y*3.0f;
        isMovementPressed = currentMovementInput.x != 0|| currentMovementInput.y != 0;
    }
    void onRun(InputAction.CallbackContext context){
        isRunPressed = context.ReadValueAsButton();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0|| currentMovementInput.y != 0;
    }

    void onJump(InputAction.CallbackContext context){
        isJumpPressed = context.ReadValueAsButton();
        Debug.Log(isJumpPressed);
    }

    void handleAnimation(){
        bool isRunning=  animator.GetBool(isRunningHash);
        bool isWalking =  animator.GetBool(isWalkingHash);

        if(!isWalking && isMovementPressed){
            animator.SetBool(isWalkingHash,true);
        }

        else if(isWalking && !isMovementPressed){
            animator.SetBool(isWalkingHash,false);
        }
        if((isMovementPressed&&isRunPressed) && !isRunning){
            animator.SetBool(isRunningHash,true);
        }
        else if((!isMovementPressed || !isRunPressed) && isRunning){
            animator.SetBool(isRunningHash,false);
        }
    }

    void handleRotation(){
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation= transform.rotation;
        if(isMovementPressed){
            Quaternion targetRotation= Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation,targetRotation,rotationfactorPerFrame*Time.deltaTime);

        }
    }

    // Update is called once per frame
    void Update()
    {
        handleRotation();
        handleAnimation();
        if(isRunPressed){
            characterController.Move(currentRunMovement*2 * Time.deltaTime); 
        }
        else{
            characterController.Move(currentMovement*2 * Time.deltaTime); 
        }
        
    }

    void OnEnable(){
        playerInput.CharacterControls.Enable();
    }

    void OnDisable(){
        playerInput.CharacterControls.Disable();
    }
}
