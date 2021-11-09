using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreTracker : MonoBehaviour
{
    string high_score_string;
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
                string path = "Assets/High Score/high_score.txt";
                StreamReader reader = new StreamReader(path);
                high_score_string = reader.ReadToEnd();
                reader.Close();
                print("current high score: " + high_score_string);
                if (high_score_string == "")
                {
                    File.WriteAllText(path, Player.score.ToString());
                }
                else if (Player.score > int.Parse(high_score_string))
                {
                    File.WriteAllText(path, Player.score.ToString());
                }
            }
        }
    }
}
