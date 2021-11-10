using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeValue : MonoBehaviour
{
    [Tooltip("True for SFX, False for Music")]
    public bool sfx;
    TextMesh volume_value;
    void Start()
    {
        volume_value = GetComponent<TextMesh>();
    }

    public void UpdateVolumeValue()
    {
        //print("sfx vol: " + VolumeManager.sfx_volume);
        //print("music vol: " + VolumeManager.music_volume);
        if (sfx)
        {
            volume_value.text = ((int)(VolumeManager.sfx_volume * 10.0f)).ToString();
        }
        else
        {
            volume_value.text = ((int)(VolumeManager.music_volume * 10.0f)).ToString();
        }
    }
}
