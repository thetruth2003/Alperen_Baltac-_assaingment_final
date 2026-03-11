using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class Player : AbstractPlayer
{
    [SerializeField] private TextMeshPro nameText;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        playerName.OnValueChanged += OnNameChanged;

        UpdateNameText();

        InitializePlayer();
    }

    public override void InitializePlayer()
    {
        health = 100;

        if (IsOwner)
        {
            SetNameServerRpc(NetworkUI.PlayerName);
        }
    }

    [ServerRpc]
    void SetNameServerRpc(string name)
    {
        playerName.Value = name;
    }

    void OnNameChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        UpdateNameText();
    }

    void UpdateNameText()
    {
        if (nameText != null)
        {
            nameText.text = playerName.Value.ToString();
        }
    }

    public override void SpawnAtPoint(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}