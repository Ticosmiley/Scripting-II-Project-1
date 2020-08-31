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


    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.0f;
    private Vector3 yVelocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool _isRunning = false;
    bool _isFalling = false;

    private void Start()
    {
        Idle?.Invoke();
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 moveDir = Vector3.zero;

        if (direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            CheckIfStoppedMoving();
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            Jump?.Invoke();
            _isRunning = false;
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
            CheckIfStartedMoving();
            if (!_isRunning)
            {
                Land?.Invoke();
            }
            _isFalling = false;
        }

        yVelocity.y += gravity * Time.deltaTime;
        moveDir += yVelocity;
        controller.Move(moveDir * Time.deltaTime);
    }

    private void CheckIfStartedMoving()
    {
        if (!_isRunning && controller.isGrounded && !_isFalling)
        {
            StartRunning?.Invoke();
            _isRunning = true;
        }
    }

    private void CheckIfStoppedMoving()
    {
        if (_isRunning)
        {
            Idle?.Invoke();
        }
        _isRunning = false;
    }
}
