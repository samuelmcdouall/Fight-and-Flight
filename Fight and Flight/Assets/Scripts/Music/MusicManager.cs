using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip boss_music;
    public AudioClip victory_music;
    AudioSource music_as;
    bool started_playing_boss_music = false;
    void Start()
    {
        music_as = GetComponent<AudioSource>();
        music_as.playOnAwake = true;
        music_as.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.score >= 50)
        {
            if (!started_playing_boss_music)
            {
                music_as.Stop();
                music_as.clip = boss_music;
                music_as.loop = true;
                music_as.Play();
                started_playing_boss_music = true;
            }
            else
            {
                // do stuff later for victory music (need to do the screen as well)
            }
        }
    }
}
