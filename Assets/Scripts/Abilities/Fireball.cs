using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Ability
{
    [SerializeField] GameObject _projectileSpawned = null;

    int _rank = 1;

    public override void Use(Transform origin, Transform target)
    {
        GameObject projectile = Instantiate(_projectileSpawned, origin.position, origin.rotation);

        if (target != null)
        {
            projectile.transform.LookAt(target);
        }

        Destroy(projectile, 3.5f);
    }
}
