using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public abstract class AbstractPlayer : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> playerName =
     new NetworkVariable<FixedString64Bytes>(
         "Player",
         NetworkVariableReadPermission.Everyone,
         NetworkVariableWritePermission.Server);
    public int health;  // Player's health


    /// <summary>
    /// Abstract method to initialize player-specific data
    /// </summary>
    public abstract void InitializePlayer();

    /// <summary>
    /// Abstract method to spawn the player at a specific point
    /// </summary>
    /// <param name="spawnPoint"></param> 
    public abstract void SpawnAtPoint(Transform spawnPoint);
}
