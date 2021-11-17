using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetting : MonoBehaviour
{
    public DifficultySettingText difficulty_setting_text;
    
    public void ChangeDifficulty()
    {
        DifficultyManager.difficulty = DifficultyManager.IncreaseDifficulty(DifficultyManager.difficulty);
        difficulty_setting_text.UpdateDifficultySettingText();
    }


}
