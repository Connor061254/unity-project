using UnityEditor.SettingsManagement;
using UnityEngine;

public class RockWeapon : MonoBehaviour, IWeapon
{

    private float nextAttackTime = 0f;

    private float range = 2f;

    public GameObject lastOwner;
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

            if (enemy != null)
            {
                if (!enemiesHitThisSwing.Contains(enemy))
                {
                    enemy.TakeDamage(10f);

                    enemiesHitThisSwing.Add(enemy);
                }
            
            }
        }
    }
}
