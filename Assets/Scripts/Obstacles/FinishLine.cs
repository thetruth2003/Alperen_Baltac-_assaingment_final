using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager; // Reference to the UIManager to update UI on win

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player crosses the finish line
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            // Call UIManager to display the winner's name
            _uiManager.DisplayWinner(player.playerName.Value.ToString());
        }
    }
}
