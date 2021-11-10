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
        sfx_volume = 0.5f;
        music_volume = 0.5f;
        MusicManager.UpdateMusicVolume();
        GameObject[] volume_values;
        volume_values = GameObject.FindGameObjectsWithTag("Volume Value");
        foreach (GameObject volume_value in volume_values)
        {
            volume_value.GetComponent<VolumeValue>().UpdateVolumeValue();
        }
    }

    public static void ChangeSFXVolume(float volume_change)
    {
        if (sfx_volume + volume_change > 1.0f)
        {
            sfx_volume = 1.0f;
        }
        else if (sfx_volume + volume_change < 0.0f)
        {
            sfx_volume = 0.0f;
        }
        else
        {
            sfx_volume += volume_change;
        }
    }

    public static void ChangeMusicVolume(float volume_change)
    {
        if (music_volume + volume_change > 1.0f)
        {
            music_volume = 1.0f;
        }
        else if (music_volume + volume_change < 0.0f)
        {
            music_volume = 0.0f;
        }
        else
        {
            music_volume += volume_change;
        }
    }
}
