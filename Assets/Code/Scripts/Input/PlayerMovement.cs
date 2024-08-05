using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Reference Variables
    private CustomInput input = null;
    private Rigidbody rb = null;
    private CharacterController characterController;
    private Animator animator;

    // Storing input values
    private Vector2 moveVector = Vector2.zero;
    private Vector3 currentMovement = Vector3.zero;
    private bool isMovementPressed;
    private bool isJumping = false;
    private bool isRunning = false;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;

    private void Awake()
    {
        input = new CustomInput();
        rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        // input.Player.Movement.started += context => { 
        //     moveVector = context.ReadValue<Vector2>();
        //     currentMovement.x = moveVector.x;
        //     currentMovement.z = moveVector.y;
        //     isMovementPressed = moveVector.x != 0 || moveVector.y != 0;
        // };
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Run.performed += OnRunPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Player.Run.performed -= OnRunPerformed;
    }

    private void FixedUpdate()
    {
        // Make the player move towards where the camera is pointing
        moveVector = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveVector;
        moveVector.Normalize();

        // Move using the RigidBody
        //rb.velocity = new Vector3(moveVector.x * moveSpeed, rb.velocity.y, moveVector.y * moveSpeed);

        // Move using character controller
        //characterController.Move(currentMovement * Time.deltaTime); // deltaTime may be unnecessary
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVector = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if(!isJumping)
        {
            Debug.Log("Jumping");
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnRunPerformed(InputAction.CallbackContext value)
    {
        if(!isRunning)
        {
            Debug.Log("Running");
            moveSpeed += 2f;
            //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void OnRunCancelled(InputAction.CallbackContext value)
    {
        isRunning = false;
        moveSpeed -= 2f;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Floor")
            isJumping = false;
    }

    private void OnTriggerExit(Collider collision)
    {
        if(collision.gameObject.tag == "Floor")
            isJumping = true;
    }

    private void OnApplicationFocus(bool focus)
    {
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void HandleAnimation()
    {
        // Get parameters
        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");

        if(isMovementPressed && !isWalking){
            animator.SetBool("isWalking", true);
        }else if (!isMovementPressed && isWalking){
            animator.SetBool("isWalking", false);
        }
    }
}
