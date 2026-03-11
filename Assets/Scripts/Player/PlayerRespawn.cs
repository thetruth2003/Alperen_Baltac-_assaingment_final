using Unity.Netcode;
using UnityEngine;

public class PlayerRespawn : NetworkBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private float respawnTime = 5f; // Time to wait before respawning
    [SerializeField] private Vector3 respawnPosition = Vector3.zero; // Respawn position

    private Rigidbody rb; // Reference to the Rigidbody
    private float timeWithoutContact = 0f; // Time spent without contacting any object

    private void Awake()
    {
        // Initialize Rigidbody
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component is required for PlayerRespawn.");
        }
    }

    private void Update()
    {
        if (!IsOwner) return; // Ensure only the owning client handles the respawn logic

        // Perform Raycast to check if player is not touching the ground
        if (!IsTouchingAnyObject())
        {
            // If no object contact, start the timer
            timeWithoutContact += Time.deltaTime;
            Debug.Log($"Time without contact: {timeWithoutContact}s");
        }
        else
        {
            // If the player is touching any object, reset the timer
            timeWithoutContact = 0f;
        }

        // If time without contact exceeds respawn time, request respawn
        if (timeWithoutContact >= respawnTime)
        {
            Debug.Log("Respawn time reached. Requesting respawn...");
            RequestRespawnServerRpc();
            timeWithoutContact = 0;
        }
    }

    /// <summary>
    /// Checks if the player is touching any object using Raycast.
    /// This returns true if player is touching any object (ground or something else).
    /// </summary>
    private bool IsTouchingAnyObject()
    {
        // Perform a Raycast downward from the player's position to check for any object contact
        RaycastHit hit;

        // We check for objects in a 3f distance (adjustable if needed).
        return Physics.Raycast(transform.position, Vector3.down, out hit, 3f);
    }

    /// <summary>
    /// Requests the server to respawn the player after the timeout.
    /// </summary>
    [ServerRpc]
    private void RequestRespawnServerRpc()
    {
        RespawnPlayerServerRpc();
    }

    /// <summary>
    /// Respawns the player at the designated respawn position.
    /// </summary>
    [ServerRpc]
    private void RespawnPlayerServerRpc()
    {
        // Make sure the server has authority to respawn
        if (IsServer)
        {
            // Reset position to respawn point
            transform.position = respawnPosition;
            Debug.Log("Player has been respawned to position: " + respawnPosition);

            // Reset velocity using Rigidbody
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Reset linear velocity
                rb.angularVelocity = Vector3.zero; // Reset angular velocity
                rb.useGravity = true; // Ensure gravity is enabled after respawn
                Debug.Log("Rigidbody velocities reset and gravity enabled.");
            }

            // Notify clients
            NotifyRespawnClientRpc();
        }
        else
        {
            Debug.LogError("Attempted to respawn player, but this is not the server.");
        }
    }

    /// <summary>
    /// Sends a respawn notification to clients (e.g., for visual effects or sounds).
    /// </summary>
    [ClientRpc]
    private void NotifyRespawnClientRpc()
    {
        Debug.Log("Player has been respawned on all clients.");
    }
}
