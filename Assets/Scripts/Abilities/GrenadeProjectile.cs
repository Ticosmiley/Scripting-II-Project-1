using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GrenadeProjectile : MonoBehaviour
{
    Rigidbody _rb;
    Player _player;
    CinemachineImpulseSource _impulse;

    [SerializeField] public float _distance = 100;
    [SerializeField] float _fuseTime = 3f;
    [SerializeField] float _radius = .5f;
    [SerializeField] float _power = 10f;
    [SerializeField] int _damage = 1;
    [SerializeField] GameObject _audioSource;
    [SerializeField] GameObject _explosion;
    Vector3 _force;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>();
        _distance = _player.Power;
        _impulse = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        _force = (transform.forward + new Vector3(0f, 0.5f, 0f)) * _distance;
        _rb.AddForce(_force);
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        yield return new WaitForSeconds(_fuseTime);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, _radius);
        foreach (Collider hit in colliders)
        {
            Enemy enemy = hit.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Health enemyHealth = enemy.GetComponent<Health>();
                enemyHealth.TakeDamage(_damage);
            }
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_power, explosionPos, _radius + 2, 3.0f);
            }
        }
        _impulse.GenerateImpulse();
        Instantiate(_audioSource, transform.position, Quaternion.identity);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
