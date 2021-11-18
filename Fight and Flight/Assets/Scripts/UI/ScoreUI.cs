using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text score_text;
    string current_level;
    void Start()
    {
        score_text = GetComponent<Text>();
    }
    void Update()
    {
        if (PlayerPCTest.boss_spawned)
        {
            current_level = "BOSS";
        }
        else
        {
            current_level = "Level: " + (PlayerPCTest.player_current_level + 1);
        }
        score_text.text = "Score: " + PlayerPCTest.score + "\n" +
                          current_level + "\n" +
                          "Drones : " + (PlayerPCTest.drones_destroyed);
    }
}
