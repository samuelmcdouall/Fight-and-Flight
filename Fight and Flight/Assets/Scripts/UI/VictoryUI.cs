using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    Text gamer_over_text;
    void OnEnable()
    {
        gamer_over_text = GetComponent<Text>();
        gamer_over_text.text = "You win!" + "\n" + "\n" +
                               "Final Score: " + Player.score + "\n" +
                               "Final Level: " + (Player.player_level + 1) + "\n" +
                               "Drones Destroyed: " + (Player.drones_destroyed);
    }
}
