using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider _healthBar;
    private Health _playerHealth;

    private void Start()
    {
        _playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
        _healthBar = GetComponent<Slider>();
        _healthBar.maxValue = _playerHealth.MaxHealth;
        _healthBar.value = _playerHealth.MaxHealth;
    }

    private void Update()
    {
        _healthBar.value = _playerHealth.CurrentHealth;
    }
}
