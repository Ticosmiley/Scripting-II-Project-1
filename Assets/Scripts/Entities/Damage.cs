using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] int _damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (!health.IFrames)
            {
                health.TakeDamage(_damageAmount);
            }
        }
    }
}
