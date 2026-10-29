using Unity.Collections.Tests.CoreCLR.TestJobs;
using Unity.Netcode;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    public void UpdatePlayerColors()
    {
        if (IsOwner) return;

        var localPlayer = NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<PlayerDataBackpack>();

        var theirBackPack = GetPlayerDataBackpack(this.OwnerClientId);



        if(localPlayer != null && theirBackPack != null)
        {
            if(localPlayer.TeamIndex.Value == theirBackPack.TeamIndex.Value)
            {
                SetLayerAllChildren(this.transform, 15);
            }
            else
            {
                SetLayerAllChildren(this.transform, 16);
            }
        }
    }

    private PlayerDataBackpack GetPlayerDataBackpack(ulong clientId)
    {
        var backPacks = FindObjectsByType<PlayerDataBackpack>();
        foreach(var backpack in backPacks)
        {
            if(backpack.OwnerClientId == clientId)
            {
                return backpack;
            }
        }
        return null;
    }

    private void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach(var child in children)
        {
            child.gameObject.layer = layer;
        }
    }
}
