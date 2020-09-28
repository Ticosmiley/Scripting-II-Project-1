using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action Jump = delegate { };
    public event Action StartFalling = delegate { };
    public event Action Land = delegate { };
    public event Action StartSprinting = delegate { };

    public CharacterController controller;
    public Transform cam;

    private Player _player;
    private Health _health;
    [SerializeField] ParticleSystem _dashParticles;
    [SerializeField] ParticleSystem _landingParticles;
    [SerializeField] AudioSource _landSound;
    [SerializeField] AudioSource[] _footsteps;

    [SerializeField] float _stepSpeed;
    [SerializeField] float _sprintStepSpeed;

    public float sprintSpeed = 2f;
    public float speed = 3f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.0f;
    private Vector3 yVelocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool _isRunning = false;
    bool _isFalling = false;
    bool _isSprinting = false;
    bool _speedBoost = false;
    bool _takingDamage = false;
    
    public bool IsSprinting { get { return _isSprinting; } set { _isSprinting = value; } }
    public bool TakingDamage { get { return _takingDamage; } set { _takingDamage = value; } }

    private void Awake()
    {
        _player = GetComponent<Player>();
        _health = GetComponent<Health>();
        _dashParticles.enableEmission = false;
    }

    private void Start()
    {
        Idle?.Invoke();
    }

    void Update()
    {
        if (!_player.IsDead)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            Vector3 moveDir = Vector3.zero;

            if (direction.magnitude >= 0.1f && _player.CanMove)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (!_isSprinting && !_takingDamage && controller.isGrounded)
                    {
                        StartSprinting?.Invoke();
                        _dashParticles.enableEmission = true;
                        _isSprinting = true;
                        StartCoroutine(SprintSteps());
                    }
                    _isRunning = false;
                }
                else
                {
                    CheckIfStartedMoving();
                    _isSprinting = false;
                }
            }
            else
            {
                CheckIfStoppedMoving();
            }

            if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded && _player.CanMove)
            {
                Jump?.Invoke();
                _isRunning = false;
                _isSprinting = false;
                yVelocity.y = 0;
                yVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }

            if (!controller.isGrounded)
            {
                if (yVelocity.y < 0 && !_isFalling)
                {
                    StartFalling?.Invoke();
                    _isFalling = true;
                }
            }

            if (_isFalling && controller.isGrounded)
            {
                _landingParticles.Emit(30);
                _landSound.PlayOneShot(_landSound.clip);
                if (!_isRunning && !_isSprinting)
                {
                    Land?.Invoke();
                }
                _isFalling = false;
            }

            if (!_isSprinting)
            {
                _dashParticles.enableEmission = false;
            }

            yVelocity.y += gravity * Time.deltaTime;
            moveDir += yVelocity;
            moveDir.x *= speed;
            moveDir.z *= speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speedBoost = true;
            }
            else
            {
                _speedBoost = false;
            }

            if (_speedBoost)
            {
                moveDir.x *= sprintSpeed;
                moveDir.z *= sprintSpeed;
            }
            controller.Move(moveDir * Time.deltaTime);
        }
    }

    private void CheckIfStartedMoving()
    {
        if ((!_isRunning || _health.TakenDamage) && controller.isGrounded && !_isFalling && !_takingDamage)
        {
            StartRunning?.Invoke();
            _isRunning = true;
            _isSprinting = false;
            _health.TakenDamage = false;
            StartCoroutine(RunningSteps());
        }
    }

    private void CheckIfStoppedMoving()
    {
        if ((_isRunning || _isSprinting || _player.HasThrown || _health.TakenDamage) && !_player.IsCharging && !_takingDamage)
        {
            Idle?.Invoke();
        }
        _isRunning = false;
        _isSprinting = false;
        _player.HasThrown = false;
        _health.TakenDamage = false;
    }

    IEnumerator RunningSteps()
    {
        while (_isRunning && !_isSprinting)
        {
            AudioSource step = _footsteps[UnityEngine.Random.Range(0, 3)];
            step.PlayOneShot(step.clip, .5f);
            yield return new WaitForSeconds(_stepSpeed);
        }
    }

    IEnumerator SprintSteps()
    {
        while (_isSprinting)
        {
            AudioSource step = _footsteps[UnityEngine.Random.Range(0, 3)];
            step.PlayOneShot(step.clip);
            yield return new WaitForSeconds(_sprintStepSpeed);
        }
    }
}
