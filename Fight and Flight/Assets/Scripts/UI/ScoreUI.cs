using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    Text score_text;
    void Start()
    {
        score_text = GetComponent<Text>();
        score_text.text = "Score: " + Player.score;
    }
    void Update()
    {
        score_text.text = "Score: " + Player.score;
    }
}
