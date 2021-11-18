using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScoreTracker : MonoBehaviour
{
    public static int high_score;

    public static void CheckAgainstCurrentHighScore()
    {
        high_score = PlayerPrefs.GetInt("High Score", -1);
        if (high_score == -1)
        {
            PlayerPrefs.SetInt("High Score", PlayerPCTest.score);
            PlayerPrefs.Save();
        }
        else if (PlayerPCTest.score > high_score)
        {
            PlayerPrefs.SetInt("High Score", PlayerPCTest.score);
            PlayerPrefs.Save();
        }
    }
}
