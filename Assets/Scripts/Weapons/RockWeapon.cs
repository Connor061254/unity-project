using Mono.CSharp;
using Unity.Netcode;
using UnityEngine;


public class RockWeapon : NetworkBehaviour, IWeapon, IWeaponThrow
{

    private float nextAttackTime = 0f;

    private float range = 2f;

    public NetworkObject lastOwner;

    private ItemData itemInfo;


    private float throwForce = 4;

    public float attackCooldown = 2f;

    public AnimationCurve damageCurve;

    public float minimumEffectiveDistance = 3f;

    public float thrownDistanceReducer = 0.4f;

    private Vector3 myThrowPosition;


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
                if (!enemiesHitThisSwing.Contains(enemy) && transform.parent.gameObject.GetComponent<Identification>().type == CharacterType.CutlassKate)
                {
                    if(GetComponent<SpecialAbility>() != null)
                    {
                        enemy.GetComponent<EffectsManager>().StartBleed();
                    }
                    else
                    {
                        enemy.TakeDamage(damage);
                    }
                    
                    

                    enemiesHitThisSwing.Add(enemy);
                }
            
            }
        }
    }

    public void ThrowAttack(NetworkObject thrower, Vector3 targetPoint, Vector3 myThrowPosition)
    {   
        PerformThrowRpc(thrower, targetPoint, myThrowPosition);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void PerformThrowRpc(NetworkObjectReference thrower, Vector3 targetPoint, Vector3 position)
    {
        if(thrower.TryGet(out NetworkObject throwerObj))
        {
            var inventory = throwerObj.GetComponent<InventoryManager>();
            var itemProp = this.gameObject.GetComponent<ItemProperties>();
            var pickupScript = throwerObj.GetComponent<OfficialPickupScript>();

            myThrowPosition = position;

            pickupScript.currentHeldObject = null;

            itemInfo = itemProp.referenceData;

            inventory.RemoveItem(itemInfo);
        
            lastOwner = throwerObj;
            

            throwForce = throwerObj.GetComponent<AttackPrep>().powerMultiplier;

            float baseThrowForce = 15f;

            float finalThrowForce = throwForce * baseThrowForce;

            var rb = GetComponent<Rigidbody>();

            this.NetworkObject.TryRemoveParent();

            if(TryGetComponent<Collider>(out var rockCollider)) rockCollider.enabled = true;
       
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;


            Vector3 throwDirection = (targetPoint - transform.position).normalized;

            throwDirection += Vector3.up * 0.3f;

            throwDirection.Normalize();

            rb.AddForce(throwDirection * finalThrowForce, ForceMode.Impulse);

            pickupScript.ClearHeldObjectOnClients(throwerObj);

            if (GetComponent<SpecialAbility>() != null && throwerObj.GetComponent<Identification>().type == CharacterType.BeanstalkBill)
        {
            StartCoroutine(GetComponent<SpecialAbility>().SplitShotTimer());
        }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(!lastOwner || collision.gameObject == lastOwner) return;

        HealthController targetHealth = collision.gameObject.GetComponent<HealthController>();

        if(targetHealth != null)
        {
            float damage = GetComponent<DamageDealer>().damageNumber;
            float speed = this.gameObject.GetComponent<Rigidbody>().linearVelocity.magnitude;

            float curveMultiplier = damageCurve.Evaluate(speed);
            float thrownDamage = curveMultiplier * damage;

            float distanceTraveled = Vector3.Distance(myThrowPosition, transform.position);

            if(distanceTraveled < minimumEffectiveDistance)
            {
                thrownDamage = thrownDamage * thrownDistanceReducer;
            }

            Debug.Log("Speed: " + speed + " | Multiplier: " + curveMultiplier + " | Damage: " + thrownDamage);

            if (lastOwner.gameObject.GetComponent<Identification>().type == CharacterType.CutlassKate)
            {
                collision.gameObject.GetComponent<EffectsManager>().StartBleed();
            }
            else
            {
                targetHealth.TakeDamage(thrownDamage);
            }
        }
        

        lastOwner = null;
    }

    public void SetThrowPosition(Vector3 position)
    {
        myThrowPosition = position;
    }
}
