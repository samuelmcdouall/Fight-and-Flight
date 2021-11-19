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
        if (Player.boss_spawned)
        {
            current_level = "BOSS";
        }
        else
        {
            current_level = "Level: " + (Player.player_current_level + 1);
        }
        score_text.text = "Score: " + Player.score + "\n" +
                          current_level + "\n" +
                          "Drones : " + (Player.drones_destroyed);
    }
}
