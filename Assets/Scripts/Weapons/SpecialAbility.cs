using System.Collections;
using Unity.Netcode;
using UnityEngine;

public enum AbilityType
{
    splitshot,
    speedIncrease,
    bleeder,
    none
}
public class SpecialAbility : NetworkBehaviour
{
    private AbilityType currentAbility = AbilityType.none;

    private PlayerController playerController;
    [SerializeField] private float specialRockSpeedBuff = 2f;

    [SerializeField] private float waitTime = 2f;

    [SerializeField] private float smallRocksCount;

    [SerializeField] private GameObject smallRockPrefab;

    [SerializeField] private float spreadForce;

    private NetworkObject throwableNetObj;

    private Vector3 heldPosition;

    

    private Rigidbody rockRb;
    public AbilityType GetAbilityType(CharacterType character)
    {
        return character switch
        {
            CharacterType.BeanstalkBill => AbilityType.splitshot,

            CharacterType.CutlassKate => AbilityType.bleeder,

            CharacterType.TubbsMcGee => AbilityType.speedIncrease,

            _ => AbilityType.none
        };
    }

    public void InitalizeRock(CharacterType character, PlayerController player)
    {
        playerController = player;
        currentAbility = GetAbilityType(character);

        if(currentAbility == AbilityType.speedIncrease)
        {
            increaseSpeed();
        }
    }
    public void Ability()
    {

        switch (currentAbility)
        {
            case AbilityType.splitshot:
            SplitShot();
            break;

            case AbilityType.bleeder:
            Bleeder();
            break;
        }
    }

    void Start()
    {
        rockRb = GetComponent<Rigidbody>();
    }

    private void SplitShot()
    {
        if (!IsServer) return;

        Debug.Log($"[SplitShot] Timer finished! Attempting to spawn {smallRocksCount} small rocks.");

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        Vector3 inheritedVelocity = rockRb.linearVelocity;

        for (int i = 0; i < smallRocksCount; i++)
        {
            Debug.Log($"[SplitShot] Spawning small rock index: {i}");
            GameObject smallRock = Instantiate(smallRockPrefab, spawnPosition, spawnRotation);
            NetworkObject netObj = smallRock.GetComponent<NetworkObject>();
            Rigidbody smallRockRb = smallRock.GetComponent<Rigidbody>();
            netObj.Spawn();
            if (smallRockRb != null)
            {
                Vector3 randomSpread = new Vector3( 
                    Random.Range(-1f, 1f),
                    Random.Range(0.2f, 1f),
                    Random.Range(-1f, 1f)
                ).normalized * spreadForce;
                
                smallRockRb.linearVelocity = inheritedVelocity + randomSpread;
            }

            
        }

        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn(true);
        }

        Destroy(gameObject);
    }

    [Rpc(SendTo.Server)]

    private void PerformSplitShotRpc()
    {
        SplitShot();
    }

    public IEnumerator SplitShotTimer()
    {
        yield return new WaitForSeconds(waitTime);
        PerformSplitShotRpc();
    }

    public void increaseSpeed()
    {
        if(playerController != null)
        {
            playerController.itemSpeedBuff = specialRockSpeedBuff;
        }
    }

    public void ReduceSpeed()
    {
        if(currentAbility == AbilityType.speedIncrease)
        {
            playerController.itemSpeedBuff = 0f;
        }
    }

    private void Bleeder()
    {
        
    }
}
