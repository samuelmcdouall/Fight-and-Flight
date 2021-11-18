using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    Text gamer_over_text;
    void OnEnable()
    {
        gamer_over_text = GetComponent<Text>();
        gamer_over_text.text = "GAME OVER" + "\n" + "\n" + 
                               "Final Score: " + PlayerPCTest.score + "\n" + 
                               "Final Level: " + (PlayerPCTest.player_current_level + 1) + "\n" + 
                               "Drones Destroyed: " + (PlayerPCTest.drones_destroyed);
    }
}
