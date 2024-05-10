using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CustomInput input = null;
    private Vector2 moveVector = Vector2.zero;
    private bool isJumping = false;
    private Rigidbody rb = null;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 5f;

    private void Awake()
    {
        input = new CustomInput();
        rb = GetComponent<Rigidbody>();

    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
        input.Player.Movement.canceled += OnMovementCancelled;
        input.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
        input.Player.Movement.canceled -= OnMovementCancelled;
        input.Player.Jump.performed -= OnJumpPerformed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(moveVector.x * moveSpeed, rb.velocity.y, moveVector.y * moveSpeed);
        
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
}
