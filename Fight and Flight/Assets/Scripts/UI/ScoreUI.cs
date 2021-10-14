using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text score_text;
    void Start()
    {
        score_text = GetComponent<Text>();
    }
    void Update()
    {
        score_text.text = "Score: " + Player.score + "\n" +
                          "Level: " + (Player.player_level + 1) + "\n" +
                          "Drones : " + (Player.drones_destroyed);
    }
}
