using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySettingText : MonoBehaviour
{
    TextMesh difficulty_setting_text;
    void Start()
    {
        difficulty_setting_text = GetComponent<TextMesh>();
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                difficulty_setting_text.text = "Difficulty: Easy" + "\n" + "\n" + "Difficulty progression is" + "\n" + "always treated as level 1";
                difficulty_setting_text.color = Color.green;
                break;
            case DifficultyManager.Difficulty.normal:
                difficulty_setting_text.text = "Difficulty: Normal" + "\n" + "\n" + "Difficulty progression is" + "\n" + "treated as normal";
                difficulty_setting_text.color = Color.yellow;
                break;
            case DifficultyManager.Difficulty.hard:
                difficulty_setting_text.text = "Difficulty: Hard" + "\n" + "\n" + "Difficulty progression is" + "\n" + "always treated as level 5";
                difficulty_setting_text.color = Color.red;
                break;
            default:
                print("defaulted, invalid value");
                difficulty_setting_text.text = "Difficulty: Easy" + "\n" + "\n" + "Difficulty progression is" + "\n" + "always treated as level 1";
                difficulty_setting_text.color = Color.green;
                break;
        }
    }

    public void UpdateDifficultySettingText()
    {
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                difficulty_setting_text.text = "Difficulty: Easy" + "\n" + "\n" + "Difficulty progression is" + "\n" + "always treated as level 1";
                difficulty_setting_text.color = Color.green;
                break;
            case DifficultyManager.Difficulty.normal:
                difficulty_setting_text.text = "Difficulty: Normal" + "\n" + "\n" + "Difficulty progression is" + "\n" + "treated as normal";
                difficulty_setting_text.color = Color.yellow;
                break;
            case DifficultyManager.Difficulty.hard:
                difficulty_setting_text.text = "Difficulty: Hard" + "\n" + "\n" + "Difficulty progression is" + "\n" + "always treated as level 5";
                difficulty_setting_text.color = Color.red;
                break;
            default:
                print("defaulted, invalid value");
                difficulty_setting_text.text = "Difficulty: Easy" + "\n" + "\n" + "Difficulty progression is" + "\n" + "always treated as level 1";
                difficulty_setting_text.color = Color.green;
                break;
        }
    }


}
