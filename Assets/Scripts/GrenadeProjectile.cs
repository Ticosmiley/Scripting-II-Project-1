using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    Rigidbody _rb;
    Player _player;

    [SerializeField] public float _distance = 100;
    [SerializeField] float _fuseTime = 3f;
    [SerializeField] float _radius = 3f;
    [SerializeField] float _power = 10f;
    [SerializeField] GameObject _audioSource;
    [SerializeField] GameObject _explosion;
    Vector3 _force;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Player>();
        _distance = _player.Power;
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
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(_power, explosionPos, _radius, 3.0f);
            }
        }
        Instantiate(_audioSource, transform.position, Quaternion.identity);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(transform.gameObject);
    }
}
