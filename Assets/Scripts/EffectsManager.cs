using System.Collections;
using Mono.CSharp;
using Unity.VisualScripting;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    private HealthController player;

    private int TotalTicks = 5;

    private int interval = 1;

    [SerializeField] private float damagePerTick = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = this.GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBleed()
    {
        StartCoroutine(DelayedDamage());
    }

    private IEnumerator DelayedDamage()
    {
        if(this.gameObject == null) yield break;

        for(int i = 0; i < TotalTicks; i++)
        {
            if(this.gameObject == null) yield break;

            player.TakeDamage(damagePerTick);

            yield return new WaitForSeconds(interval);
        }
    }
}
