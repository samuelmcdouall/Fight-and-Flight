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
        if (sfx)
        {
            volume_value.text = (Mathf.RoundToInt(VolumeManager.sfx_volume * 10.0f)).ToString();
        }
        else
        {
            volume_value.text = (Mathf.RoundToInt(VolumeManager.music_volume * 10.0f)).ToString();
        }
    }

    public void UpdateVolumeValue()
    {
        if (sfx)
        {
            volume_value.text = (Mathf.RoundToInt(VolumeManager.sfx_volume * 10.0f)).ToString();
        }
        else
        {
            volume_value.text = (Mathf.RoundToInt(VolumeManager.music_volume * 10.0f)).ToString();
        }
    }
}
