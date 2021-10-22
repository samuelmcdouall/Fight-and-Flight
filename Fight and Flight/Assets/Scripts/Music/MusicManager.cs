using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip menu_music;
    public AudioClip game_music;
    public AudioClip boss_music;
    public AudioClip victory_music;
    AudioSource music_as;
    bool started_playing_menu_music;
    bool started_playing_game_music;
    bool started_playing_boss_music;
    bool started_victory_music_gap;
    bool started_playing_victory_music;
    void Start()
    {
        InitialMusicManagerSetup();
    }

    void Update()
    {
        if (Player.in_menu)
        {
            if (!started_playing_menu_music)
            {
                music_as.Stop();
                music_as.clip = menu_music;
                music_as.loop = true;
                music_as.Play();
                started_playing_menu_music = true;
            }
        }
        else if (Player.score < 50)
        {
            if (!started_playing_game_music)
            {
                music_as.Stop();
                music_as.clip = game_music;
                music_as.loop = true;
                music_as.Play();
                started_playing_game_music = true;
            }
        }
        else
        {
            if (!started_playing_boss_music)
            {
                music_as.Stop();
                music_as.clip = boss_music;
                music_as.loop = true;
                music_as.Play();
                started_playing_boss_music = true;
            }
            if (!Player.victory && Player.victory_countdown_begun)
            {
                if (!started_victory_music_gap)
                {
                    started_victory_music_gap = false;
                    music_as.Stop();
                }
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

    private void InitialMusicManagerSetup()
    {
        music_as = GetComponent<AudioSource>();
        music_as.playOnAwake = true;
        music_as.loop = true;
        started_playing_menu_music = false;
        started_playing_game_music = false;
        started_playing_boss_music = false;
        started_victory_music_gap = false;
        started_playing_victory_music = false;
    }
}
