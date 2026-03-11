using Unity.Netcode;
using UnityEngine;

public class NetworkKnockbackObject : NetworkBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float knockbackMultiplier = 2f;
    [SerializeField] private float minKnockback = 5f;
    [SerializeField] private float maxKnockback = 20f;
    [SerializeField] private float upwardForce = 2f;

    private Rigidbody objectRb;

    private void Awake()
    {
        // Get rigidbody of this object
        objectRb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only server applies physics in multiplayer
        if (!IsServer) return;

        // Check if the collided object is the player
        if (!collision.collider.CompareTag("Player")) return;

        NetworkObject playerNetworkObject = collision.collider.GetComponent<NetworkObject>();
        if (playerNetworkObject == null) return;

        ApplyKnockbackToPlayer(playerNetworkObject, collision);
    }

    private void ApplyKnockbackToPlayer(NetworkObject playerNetworkObject, Collision collision)
    {
        Rigidbody playerRb = playerNetworkObject.GetComponent<Rigidbody>();
        if (playerRb == null) return;

        Vector3 direction;

        // Use object velocity as main knockback direction
        if (objectRb != null && objectRb.linearVelocity.sqrMagnitude > 0.01f)
        {
            direction = objectRb.linearVelocity.normalized;
        }
        else
        {
            // Fallback direction if object is almost not moving
            direction = (playerRb.position - transform.position).normalized;
        }

        // Add a small upward effect
        direction.y += 0.25f;
        direction.Normalize();

        // Calculate force based on hit speed
        float impactSpeed = objectRb != null ? objectRb.linearVelocity.magnitude : 1f;
        float finalForce = Mathf.Clamp(impactSpeed * knockbackMultiplier, minKnockback, maxKnockback);

        // Apply knockback to the player
        playerRb.AddForce(direction * finalForce + Vector3.up * upwardForce, ForceMode.Impulse);

        // Send info to clients for debug or visual feedback
        KnockbackClientRpc(playerNetworkObject.NetworkObjectId, direction * finalForce);
    }

    [ClientRpc]
    private void KnockbackClientRpc(ulong playerNetworkObjectId, Vector3 appliedForce)
    {
        // Can be used later for effects or sounds
        Debug.Log($"Player {playerNetworkObjectId} knocked back with force {appliedForce}");
    }
}