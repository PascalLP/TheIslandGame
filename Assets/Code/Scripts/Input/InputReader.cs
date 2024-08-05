using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, CustomInput.IPlayerActions
{
    public Vector2 MouseDelta;
    public Vector2 MoveComposite;

    public Action OnJumpPerformed;

    private CustomInput inputs;

    private void OnEnable()
    {
        if (inputs != null)
            return;

        inputs = new CustomInput();
        inputs.Player.SetCallbacks(this);
        inputs.Player.Enable();
    }

    private void OnDisable()
    {
        inputs.Player.Disable();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        OnJumpPerformed?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        MouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MoveComposite = context.ReadValue<Vector2>();
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        //OnRunPerformed?.Invoke();
    }
}
