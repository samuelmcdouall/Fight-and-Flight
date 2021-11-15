using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreTracker : MonoBehaviour
{
    int high_score;
    bool checked_high_score;

    private void Start()
    {
        checked_high_score = false;
    }
    void Update()
    {
        if (Player.game_over || Player.victory)
        {
            if (!checked_high_score)
            {
                checked_high_score = true;
                high_score = PlayerPrefs.GetInt("High Score", -1);
                if (high_score == -1)
                {
                    PlayerPrefs.SetInt("High Score", Player.score);
                    PlayerPrefs.Save();
                }
                else if (Player.score > high_score)
                {
                    PlayerPrefs.SetInt("High Score", Player.score);
                    PlayerPrefs.Save();
                }
            }
        }
    }
}
