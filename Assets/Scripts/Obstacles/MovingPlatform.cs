using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class MovingPlatform : NetworkBehaviour
{
    public enum MovementDirection
    {
        Horizontal, // Movement along the Z axis
        Vertical    // Movement along the Y axis
    }

    [Header("Platform Movement Settings")]
    [SerializeField] private MovementDirection direction = MovementDirection.Horizontal; // Direction of movement
    [SerializeField] private float minPosition = 0f; // Minimum position value
    [SerializeField] private float maxPosition = 5f; // Maximum position value
    [SerializeField] private float speed = 2f; // Movement speed

    [Header("Random Delay Settings")]
    [SerializeField] private float minDelay = 1f; // Minimum delay time
    [SerializeField] private float maxDelay = 2f; // Maximum delay time

    private bool movingForward = true; // Movement direction (forward or backward)
    private bool isWaiting = false; // Indicates if the platform is currently waiting

    private void Update()
    {
        if (!NetworkManager.Singleton || !NetworkManager.Singleton.IsServer)
            return;

        if (!isWaiting)
        {
            MovePlatform();
        }
    }

    /// <summary>
    /// Handles platform movement based on the selected direction.
    /// </summary>
    private void MovePlatform()
    {
        Vector3 currentPosition = transform.position;

        // Determine which axis to move along based on the direction
        if (direction == MovementDirection.Horizontal)
        {
            MoveAlongAxis(ref currentPosition.x, minPosition, maxPosition); // Move along X axis for Horizontal
        }
        else if (direction == MovementDirection.Vertical)
        {
            MoveAlongAxis(ref currentPosition.y, minPosition, maxPosition); // Move along Y axis for Vertical
        }

        // Update position and send to clients
        transform.position = currentPosition;
        SyncPositionWithClients(currentPosition);
    }

    /// <summary>
    /// Moves the platform along the specified axis with random delay at minPosition.
    /// </summary>
    /// <param name="currentAxisValue">The current position along the axis.</param>
    /// <param name="minValue">The minimum limit of movement.</param>
    /// <param name="maxValue">The maximum limit of movement.</param>
    private void MoveAlongAxis(ref float currentAxisValue, float minValue, float maxValue)
    {
        if (movingForward)
        {
            currentAxisValue = Mathf.MoveTowards(currentAxisValue, maxValue, speed * Time.deltaTime);
            if (Mathf.Approximately(currentAxisValue, maxValue))
            {
                movingForward = false; // Reverse direction
            }
        }
        else
        {
            currentAxisValue = Mathf.MoveTowards(currentAxisValue, minValue, speed * Time.deltaTime);
            if (Mathf.Approximately(currentAxisValue, minValue))
            {
                StartCoroutine(HandleRandomDelay()); // Wait before moving forward
            }
        }
    }

    /// <summary>
    /// Handles random delay at minPosition before resuming movement.
    /// </summary>
    private IEnumerator HandleRandomDelay()
    {
        isWaiting = true; // Prevent further updates during the delay

        // Generate a random delay
        float delay = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);

        movingForward = true; // Resume moving forward
        isWaiting = false; // Allow movement to continue
    }

    /// <summary>
    /// Sends the updated position to all clients.
    /// </summary>
    /// <param name="newPosition">The new position of the platform.</param>
    private void SyncPositionWithClients(Vector3 newPosition)
    {
        if (!NetworkManager.Singleton.IsListening)
            return;

        UpdatePlatformPositionClientRpc(newPosition);
    }

    /// <summary>
    /// Updates platform position on clients.
    /// </summary>
    /// <param name="newPosition">The updated position of the platform.</param>
    [ClientRpc]
    private void UpdatePlatformPositionClientRpc(Vector3 newPosition)
    {
        if (!IsServer)
        {
            transform.position = newPosition;
        }
    }
}
