using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Handles player movement in a networked environment.
/// </summary>
public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f; // Movement speed of the player
    [SerializeField] private float _jumpForce = 5f; // Jump force
    [SerializeField] private LayerMask _groundLayer; // Layer mask to detect ground

    private Rigidbody _rigidbody;
    private bool _isGrounded;

    private void Awake()
    {
        // Ensure the player has a Rigidbody component
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("Rigidbody component is missing!");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        Transform spawnPoint = SpawnManager.Instance.GetRandomSpawnPoint();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }

    /// <summary>
    /// Unity's Update method, called once per frame.
    /// </summary>
    private void Update()
    {
        if (!IsOwner) return; // Ensure only the owner can control this object

        HandleMovement();
        HandleJump();
    }

    /// <summary>
    /// Handles player movement based on input.
    /// </summary>
    private void HandleMovement()
    {
        float horizontal = GetHorizontalInput();
        float vertical = GetVerticalInput();

        Vector3 movement = CalculateMovement(horizontal, vertical);
        ApplyMovement(movement);
    }

    /// <summary>
    /// Handles player jump based on input and ground check.
    /// </summary>
    private void HandleJump()
    {
        _isGrounded = CheckGrounded();

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Retrieves horizontal input from the user.
    /// </summary>
    /// <returns>Horizontal input value.</returns>
    private float GetHorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// Retrieves vertical input from the user.
    /// </summary>
    /// <returns>Vertical input value.</returns>
    private float GetVerticalInput()
    {
        return Input.GetAxis("Vertical");
    }

    /// <summary>
    /// Calculates the movement vector based on input and speed.
    /// </summary>
    /// <param name="horizontal">Horizontal input value.</param>
    /// <param name="vertical">Vertical input value.</param>
    /// <returns>Calculated movement vector.</returns>
    private Vector3 CalculateMovement(float horizontal, float vertical)
    {
        return new Vector3(horizontal, 0, vertical) * _speed * Time.deltaTime;
    }

    /// <summary>
    /// Applies the calculated movement to the player's position.
    /// </summary>
    /// <param name="movement">Movement vector to apply.</param>
    private void ApplyMovement(Vector3 movement)
    {
        transform.Translate(movement, Space.World);
    }

    /// <summary>
    /// Checks if the player is on the ground.
    /// </summary>
    /// <returns>True if grounded, otherwise false.</returns>
    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, _groundLayer);
    }

    /// <summary>
    /// Sets the player's movement speed.
    /// </summary>
    /// <param name="newSpeed">New movement speed value.</param>
    public void SetSpeed(float newSpeed)
    {
        _speed = Mathf.Max(0, newSpeed); // Ensure speed is not negative
    }

    /// <summary>
    /// Gets the current movement speed.
    /// </summary>
    /// <returns>Current movement speed.</returns>
    public float GetSpeed()
    {
        return _speed;
    }

    /// <summary>
    /// Sets the player's jump force.
    /// </summary>
    /// <param name="newJumpForce">New jump force value.</param>
    public void SetJumpForce(float newJumpForce)
    {
        _jumpForce = Mathf.Max(0, newJumpForce); // Ensure jump force is not negative
    }

    /// <summary>
    /// Gets the current jump force.
    /// </summary>
    /// <returns>Current jump force.</returns>
    public float GetJumpForce()
    {
        return _jumpForce;
    }
}
