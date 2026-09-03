using Unity.Netcode.Components;
using UnityEngine;

[DisallowMultipleComponent]
public class ClientNetworkTransform : NetworkTransform
{
    // This single line forces the network to accept the driver's movement
    protected override bool OnIsServerAuthoritative()
    {
        return false;
    }
}
