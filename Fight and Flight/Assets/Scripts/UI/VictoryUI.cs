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
                               "Final Score: " + PlayerPCTest.score + "\n" +
                               "Drones Destroyed: " + (PlayerPCTest.drones_destroyed);
    }
}
