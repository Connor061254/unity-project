using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Chest : NetworkBehaviour
{
    public GameObject Sword;

    public GameObject Cannon;

    private GameObject objectToSpawn;
    List<GameObject> SpecialWeapon = new List<GameObject>();
    public void OpenChest()
    {
     
        SpecialWeapon.Add(Sword);
        SpecialWeapon.Add(Cannon);

        int randomIndex = UnityEngine.Random.Range(0, SpecialWeapon.Count);


        objectToSpawn = SpecialWeapon[randomIndex];

        GameObject spawnedItem = Instantiate(objectToSpawn, gameObject.transform.position + Vector3.forward * 1.5f, quaternion.identity);
        NetworkObject netObj = spawnedItem.GetComponent<NetworkObject>();

        netObj.Spawn();
    }

}
