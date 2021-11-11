using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Difficulty difficulty;
    void Start()
    {
        difficulty = Difficulty.easy;
        DontDestroyOnLoad(gameObject);
    }

    public static Difficulty IncreaseDifficulty(Difficulty current_difficulty)
    {
        switch (current_difficulty)
        {
            case Difficulty.easy:
                return Difficulty.normal;
            case Difficulty.normal:
                return Difficulty.hard;
            case Difficulty.hard:
                return Difficulty.easy;
            default:
                print("defaulted, invalid value");
                return Difficulty.easy;
        }
                
    }

    public enum Difficulty{
        easy,
        normal,
        hard
    }
}
