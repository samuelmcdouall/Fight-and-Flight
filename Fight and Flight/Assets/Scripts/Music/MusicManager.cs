using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip boss_music;
    public AudioClip victory_music;
    AudioSource music_as;
    bool started_playing_boss_music = false;
    bool started_playing_victory_music = false;
    void Start()
    {
        music_as = GetComponent<AudioSource>();
        music_as.playOnAwake = true;
        music_as.loop = true;
    }
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
            if (Player.victory)
            {
                if (!started_playing_victory_music)
                {
                    music_as.Stop();
                    music_as.clip = victory_music;
                    music_as.loop = true;
                    music_as.Play();
                    started_playing_victory_music = true;
                }
            }
        }
    }
}
