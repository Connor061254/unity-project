using Unity.Netcode;
using UnityEngine;

public interface IWeaponThrow 
{
     void ThrowAttack(NetworkObject thrower, Vector3 targetPoint, Vector3 throwPosition);
}
