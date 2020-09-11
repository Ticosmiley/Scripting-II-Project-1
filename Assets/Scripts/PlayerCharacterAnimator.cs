using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;

    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string LandState = "Landing";
    const string SprintState = "Sprint";
    const string ChargeState = "Charge";
    const string ThrowState = "Throw";

    Animator _animator = null;
    Player _player = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.Jump += OnJump;
        _thirdPersonMovement.StartFalling += OnStartFalling;
        _thirdPersonMovement.Land += OnLand;
        _thirdPersonMovement.StartSprinting += OnStartSprinting;
        _player.Charging += OnCharging;
        _player.Throw += OnThrow;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle -= OnIdle;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.Jump -= OnJump;
        _thirdPersonMovement.StartFalling -= OnStartFalling;
        _thirdPersonMovement.Land -= OnLand;
        _thirdPersonMovement.StartSprinting -= OnStartSprinting;
        _player.Charging -= OnCharging;
        _player.Throw -= OnThrow;
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    private void OnJump()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }

    private void OnStartFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    private void OnLand()
    {
        _animator.CrossFadeInFixedTime(LandState, .2f);
    }

    private void OnStartSprinting()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);
    }

    private void OnCharging()
    {
        _animator.CrossFadeInFixedTime(ChargeState, .2f);
    }

    private void OnThrow()
    {
        _animator.CrossFadeInFixedTime(ThrowState, .2f);
        StartCoroutine(ThrowToIdle());
    }

    IEnumerator ThrowToIdle()
    {
        yield return new WaitForSeconds(0.433f);
        _player.HasThrown = true;
        _player.CanMove = true;
        _player.Power = 100f;
    }
}
