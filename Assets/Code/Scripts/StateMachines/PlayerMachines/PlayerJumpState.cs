using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("isJumping");
    private const float CrossFadeDuration = 0.1f;
    private bool isJumping = true;

    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Velocity = new Vector3(stateMachine.Velocity.x, stateMachine.JumpForce, stateMachine.Velocity.z);

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
        stateMachine.Animator.SetBool(JumpHash, isJumping);
    }

    public override void Tick()
    {
        ApplyGravity();

        if (stateMachine.Velocity.y <= 0f)
        {
            isJumping = false;
            stateMachine.Animator.SetBool(JumpHash, isJumping);
            stateMachine.SwitchState(new PlayerFallState(stateMachine));
        }

        FaceMoveDirection();
        Move();

        
    }

    public override void Exit() { }
}
