using TMPro;
using UnityEngine;
using UnityEngine.UI; // For UI Text components

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _winText; // Reference to the UI Text to display the winner

    // Display the winner's name on the UI
    public void DisplayWinner(string playerName)
    {
        if (_winText != null)
        {
            _winText.text = $"{playerName} won!"; // Update the UI with the winner's name
            Debug.Log($"{playerName} won!"); // Print to console for debugging
        }
    }
}
