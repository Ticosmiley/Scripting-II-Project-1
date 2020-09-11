using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grenade : Ability
{
    [SerializeField] GameObject _projectileSpawned = null;

    public override void Use(Transform origin, Transform target)
    {
        StartCoroutine(Spawn(origin, target));
    }

    IEnumerator Spawn(Transform origin, Transform target)
    {
        yield return new WaitForSeconds(.2f);
        GameObject projectile = Instantiate(_projectileSpawned, origin.position, origin.rotation);

        if (target != null)
        {
            projectile.transform.LookAt(target);
        }
    }
}
