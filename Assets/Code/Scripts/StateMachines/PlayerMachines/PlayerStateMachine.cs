using UnityEngine;

[RequireComponent(typeof(InputReader))]
[RequireComponent (typeof(Animator))]
[RequireComponent(typeof(CharacterController))]

public class PlayerStateMachine : StateMachine
{
    public int Speed = 2;

    public Vector3 Velocity;
    public float MovementSpeed { get; private set; } = 5f;
    public float JumpForce { get; private set; } = 5f;
    public float LookRotationDampFactor { get; private set; } = 15f;
    public Transform MainCamera { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }

    void Start()
    {
        MainCamera = Camera.main.transform;
        
        InputReader = GetComponent<InputReader>();
        Animator = GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();

        SwitchState(new PlayerMoveState(this));
    }

}
