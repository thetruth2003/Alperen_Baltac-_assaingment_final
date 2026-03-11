using Unity.Netcode;
using UnityEngine;

public class PlayerCameraHandler : NetworkBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Camera _playerCamera; // Oyuncuya özel kamera

    private void Start()
    {
        // Sadece yerel oyuncunun kamerası aktif edilir
        if (IsOwner)
        {
            ActivateCamera();
        }
        else
        {
            DeactivateCamera();
        }
    }

    /// <summary>
    /// Kamerayı aktif hale getirir.
    /// </summary>
    private void ActivateCamera()
    {
        if (_playerCamera != null)
        {
            _playerCamera.enabled = true; // Kamerayı aç
        }
    }

    /// <summary>
    /// Kamerayı devre dışı bırakır.
    /// </summary>
    private void DeactivateCamera()
    {
        if (_playerCamera != null)
        {
            _playerCamera.enabled = false; // Kamerayı kapat
        }
    }
}
