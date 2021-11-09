using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    TextMesh high_score_tm;
    string high_score_string;
    void Start()
    {
        high_score_tm = GetComponent<TextMesh>();
        // read in
        string path = "Assets/High Score/high_score.txt";
        StreamReader reader = new StreamReader(path);
        high_score_string = reader.ReadToEnd();
        reader.Close();
        if (high_score_string == "")
        {
            high_score_tm.text = "High Score: --";
        }
        else
        {
            high_score_tm.text = "High Score: " + high_score_string;
        }
    }
}
