using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class Health : MonoBehaviour, IDamageable
{
    public event Action Damaged = delegate { };
    public event Action Die = delegate { };

    int _currentHealth;
    [SerializeField] float _invinceDuration = 1f;
    bool _iFrames = false;
    bool _takenDamage = false;
    [SerializeField] int _maxHealth = 5;

    [SerializeField] AudioSource _damageSound;
    [SerializeField] AudioSource _deathSound;

    CinemachineImpulseSource _impulse;

    public bool IFrames { get { return _iFrames; } }
    public bool TakenDamage { get { return _takenDamage; } set { _takenDamage = value; } }
    public int CurrentHealth { get { return _currentHealth; } }
    public int MaxHealth { get { return _maxHealth; } }

    void Awake()
    {
        _currentHealth = _maxHealth;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }
     
    public void TakeDamage(int amount)
    {
        if (_impulse != null)
        {
            _impulse.GenerateImpulse();
        }
        _iFrames = true;
        StartCoroutine(IFramesWait());
        _currentHealth -= amount;
        if (_currentHealth <= 0)
        {
            Kill();
        }
        else
        {
            if (_damageSound != null)
            {
                Instantiate(_damageSound, transform.position, Quaternion.identity, transform);
            }
            Damaged?.Invoke();
        }
    }

    public void Kill()
    {
        if (_deathSound != null)
        {
            Instantiate(_deathSound, transform.position, Quaternion.identity, transform);
        }
        Die?.Invoke();
        Destroy(this);
    }

    public void Heal(int amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth)
        {
            _currentHealth = _maxHealth;
        }
    }

    IEnumerator IFramesWait()
    {
        yield return new WaitForSeconds(_invinceDuration);
        _iFrames = false;
    }
}
