using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject Target;

    public int Speed;
    public int Damage;

    public void SetProjectile(GameObject target, int damage) {
        Target = target;
        Damage = damage;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.fixedDeltaTime);
        transform.up = Target.transform.position - transform.position;
    }
    void OnTriggerEnter2D(Collider2D other) { 
        Debug.Log("Collision");
        if (other.gameObject == Target) {
            Target.GetComponent<Unit>().Damage(Damage);
            Destroy(gameObject);
        }
    }
}
