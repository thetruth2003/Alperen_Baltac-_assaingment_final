using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : MonoBehaviour
{
    [SerializeField] private Button _hostButton; // Host Game button
    [SerializeField] private Button _joinButton; // Join Game button
    [SerializeField] private TMP_InputField _nameInput;

    public static string PlayerName;

    private void Start()
    {
        // Add listeners to the buttons
        _hostButton.onClick.AddListener(()=> { SaveName(); StartHost(); Hide();});
        _joinButton.onClick.AddListener(() => { SaveName(); StartClient(); Hide();});
    }
    void SaveName()
    {
        if (_nameInput != null && !string.IsNullOrEmpty(_nameInput.text))
        {
            PlayerName = _nameInput.text;
        }
        else
        {
            PlayerName = "Player" + Random.Range(1, 1000);
        }
    }
    /// <summary>
    /// Starts the game as a host.
    /// </summary>
    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    /// <summary>
    /// Joins the game as a client.
    /// </summary>
    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
    void Hide()
    {
        _hostButton.gameObject.SetActive(false);
        _joinButton.gameObject.SetActive(false);
        if (_nameInput) _nameInput.gameObject.SetActive(false);
    }
}
