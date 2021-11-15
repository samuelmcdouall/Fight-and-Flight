using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    TextMesh high_score_tm;
    int high_score;
    void Start()
    {
        high_score_tm = GetComponent<TextMesh>();
        high_score = PlayerPrefs.GetInt("High Score", -1);
        if (high_score == -1)
        {
            high_score_tm.text = "High Score: --";
        }
        else
        {
            high_score_tm.text = "High Score: " + high_score;
        }
    }
}
