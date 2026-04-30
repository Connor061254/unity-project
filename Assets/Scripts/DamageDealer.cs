using JetBrains.Annotations;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{


    public float damageMultiplier = 6f;
    public float damageNumber;

    void Start()
    {
        float mass = GetComponent<Rigidbody>().mass;
        damageNumber = mass * damageMultiplier;
    }






}
