using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public event Action Charging = delegate { };
    public event Action Throw = delegate { };

    [SerializeField] AbilityLoadout _abilityLoadout = null;
    [SerializeField] Ability _startingAbility = null;
    [SerializeField] Ability _newAbilityToTest = null;

    [SerializeField] Transform _testTarget = null;

    public ThirdPersonMovement _thirdPersonMovement;

    AudioSource _audioSource;

    bool _isCharging = false;
    bool _hasThrown = false;
    bool _canMove = true;
    float _power = 100f;
    float _lastThrow;
    public bool CanMove { get { return _canMove; } set { _canMove = value; } }
    public float Power { get { return _power; } set { _power = value; } }
    public bool HasThrown { get { return _hasThrown; } set { _hasThrown = value; } }
    public bool IsCharging { get { return _isCharging; } }

    public Transform CurrentTarget { get; private set; }

    private void Awake()
    {
        if (_startingAbility != null)
        {
            _abilityLoadout?.EquipAbility(_startingAbility);
        }
        _thirdPersonMovement = GetComponent<ThirdPersonMovement>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetTarget(Transform newTarget)
    {
        CurrentTarget = newTarget;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _thirdPersonMovement.controller.isGrounded && (Time.fixedTime - 0.5f > _lastThrow))
        {
            _audioSource.Play();
            _lastThrow = Time.fixedTime;
            Charging?.Invoke();
            _canMove = false;
            _isCharging = true;
        }
        if (Input.GetMouseButton(0) && _thirdPersonMovement.controller.isGrounded)
        {
            _power++;
        }
        if ((Input.GetMouseButtonUp(0) || _power >= 600) && _isCharging)
        {
            Throw?.Invoke();
            _abilityLoadout.UseEquippedAbility(CurrentTarget);
            _isCharging = false;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _abilityLoadout.EquipAbility(_newAbilityToTest);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetTarget(_testTarget);
        }
    }
}
