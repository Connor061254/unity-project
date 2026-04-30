using System;
using UnityEditor.SettingsManagement;
using UnityEngine;

public class RockWeapon : MonoBehaviour, IWeapon, IWeaponThrow
{

    private float nextAttackTime = 0f;

    private float range = 2f;

    public GameObject lastOwner;


    private float throwForce = 4;


    public void Attack()
    {
         float delay = 1f;
        Vector3 attackPoint = transform.position + (transform.forward * range);
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint, 1f);
        nextAttackTime = UnityEngine.Time.time + delay;

        var enemiesHitThisSwing = new System.Collections.Generic.List<HealthController>();

        

        foreach (Collider hit in hitColliders)
        {
            Debug.Log("I hit: " + hit.name + " on object: " + hit.transform.root.name);
            var enemy = hit.GetComponentInParent<HealthController>();
            float damage = GetComponent<DamageDealer>().damageNumber;

            if (enemy != null)
            {
                if (!enemiesHitThisSwing.Contains(enemy))
                {
                    enemy.TakeDamage(damage);

                    enemiesHitThisSwing.Add(enemy);
                }
            
            }
        }
    }

    public void ThrowAttack(GameObject thrower, Vector3 targetPoint)
    {   
        lastOwner = thrower;

        throwForce = thrower.GetComponent<AttackPrep>().powerMultiplier;

        float baseThrowForce = 20f;

        float finalThrowForce = throwForce * baseThrowForce;

        var rb = GetComponent<Rigidbody>();

        transform.SetParent(null);
       
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        Vector3 throwDirection = (targetPoint - transform.position).normalized;

        rb.AddForce(throwDirection * finalThrowForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!lastOwner)
        {
            return;
        }
        if(collision.gameObject == lastOwner)
        {
            return;
        }
        if (collision.gameObject.GetComponent<HealthController>())
        {
            collision.gameObject.GetComponent<HealthController>().TakeDamage(10f);
        }

        lastOwner = null;
    }
}
