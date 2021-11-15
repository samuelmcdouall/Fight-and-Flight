using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static VolumeManager instance;
    public static float sfx_volume;
    public static float music_volume;
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        sfx_volume = PlayerPrefs.GetFloat("SFX Volume", 0.5f);
        music_volume = PlayerPrefs.GetFloat("Music Volume", 0.5f);
    }

    public static void ChangeSFXVolume(float volume_change)
    {
        if (sfx_volume + volume_change > 1.0f)
        {
            sfx_volume = 1.0f;
            PlayerPrefs.SetFloat("SFX Volume", sfx_volume);
            PlayerPrefs.Save();
        }
        else if (sfx_volume + volume_change < 0.0f)
        {
            sfx_volume = 0.0f;
            PlayerPrefs.SetFloat("SFX Volume", sfx_volume);
            PlayerPrefs.Save();
        }
        else
        {
            sfx_volume += volume_change;
            PlayerPrefs.SetFloat("SFX Volume", sfx_volume);
            PlayerPrefs.Save();
        }
    }

    public static void ChangeMusicVolume(float volume_change)
    {
        if (music_volume + volume_change > 1.0f)
        {
            music_volume = 1.0f;
            PlayerPrefs.SetFloat("Music Volume", music_volume);
            PlayerPrefs.Save();
        }
        else if (music_volume + volume_change < 0.0f)
        {
            music_volume = 0.0f;
            PlayerPrefs.SetFloat("Music Volume", music_volume);
            PlayerPrefs.Save();
        }
        else
        {
            music_volume += volume_change;
            PlayerPrefs.SetFloat("Music Volume", music_volume);
            PlayerPrefs.Save();
        }
    }
}
