using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardOverTime : MonoBehaviour
{
    [SerializeField] float _forwardSpeed = 0.3f;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(transform.position + transform.forward * _forwardSpeed);
    }
}
