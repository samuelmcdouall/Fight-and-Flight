using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControl : MonoBehaviour
{
    public VolumeValue volume_value;
    [SerializeField]
    [Tooltip("True for Plus, False for Minus")]
    bool plus;

    public void ChangeVolume()
    {
        if (plus)
        {
            if (volume_value.sfx)
            {
                VolumeManager.ChangeSFXVolume(0.1f);
            }
            else
            {
                VolumeManager.ChangeMusicVolume(0.1f);
                MusicManager.UpdateMusicVolume();
            }
        }
        else
        {
            if (volume_value.sfx)
            {
                VolumeManager.ChangeSFXVolume(-0.1f);
            }
            else
            {
                VolumeManager.ChangeMusicVolume(-0.1f);
                MusicManager.UpdateMusicVolume();
            }
        }
        volume_value.UpdateVolumeValue();
    }

    
}
