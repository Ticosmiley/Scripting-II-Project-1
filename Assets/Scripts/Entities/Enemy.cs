using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player _player;
    Rigidbody _rb;
    Health _health;

    [SerializeField] float _speed = 5;
    [SerializeField] float _fadeSpeed = .1f;

    bool _grounded;

    private void Awake()
    {
        _player = FindObjectOfType<Player>();
        _rb = GetComponent<Rigidbody>();
        _health = GetComponent<Health>();
        _grounded = false;
    }

    private void OnEnable()
    {
        _health.Die += OnDeath;
    }

    private void OnDisable()
    {
        _health.Die -= OnDeath;
    }

    private void FixedUpdate()
    {
        if (_grounded)
        {
            Vector3 force = (_player.transform.position - transform.position).normalized * _speed * Time.deltaTime;
            force.y = 0;
            _rb.AddForce(force);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _grounded = true;
    }

    void OnDeath()
    {
        Destroy(gameObject.GetComponent<Damage>());
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        Color color = gameObject.GetComponent<MeshRenderer>().material.color;
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * _fadeSpeed;
            gameObject.GetComponent<MeshRenderer>().material.color = color;
            yield return null;
        }
        Destroy(gameObject);
    }
}
