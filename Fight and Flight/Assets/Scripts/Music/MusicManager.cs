using Unity.RemoteConfig;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Audio
    public AudioClip menu_music;
    public AudioClip game_music;
    public AudioClip boss_music;
    public AudioClip victory_music;
    public AudioClip xmas_menu_music;
    public AudioClip xmas_game_music;
    public AudioClip xmas_boss_music;
    public AudioClip xmas_victory_music;
    AudioClip currently_selected_menu_music;
    AudioClip currently_selected_game_music;
    AudioClip currently_selected_boss_music;
    AudioClip currently_selected_victory_music;
    public static AudioSource music_as;

    // Logic
    bool started_playing_menu_music;
    bool started_playing_game_music;
    bool started_playing_boss_music;
    bool started_victory_music_gap;
    bool started_playing_victory_music;

    // Remote Config
    public struct user_attributes { }
    public struct app_attributes { }
    void Start()
    {
        InitialMusicManagerSetup();
    }

    void Update()
    {
        
        if (Player.in_menu)
        {
            if (!started_playing_menu_music && currently_selected_menu_music)
            {
                music_as.Stop();
                music_as.clip = currently_selected_menu_music;
                music_as.loop = true;
                music_as.Play();
                started_playing_menu_music = true;
            }
        }
        else if (Player.score < 50)
        {
            if (!started_playing_game_music && currently_selected_game_music)
            {
                music_as.Stop();
                music_as.clip = currently_selected_game_music;
                music_as.loop = true;
                music_as.Play();
                started_playing_game_music = true;
            }
        }
        else
        {
            if (!started_playing_boss_music && currently_selected_boss_music)
            {
                music_as.Stop();
                music_as.clip = currently_selected_boss_music;
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
                if (!started_playing_victory_music && currently_selected_victory_music)
                {
                    music_as.Stop();
                    music_as.clip = currently_selected_victory_music;
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
        ChooseMusic();

    }

    void ChooseMusic()
    {
        if (RemoteConfigSettings.instance.xmas)
        {
            currently_selected_menu_music = xmas_menu_music;
            currently_selected_game_music = xmas_game_music;
            currently_selected_boss_music = xmas_boss_music;
            currently_selected_victory_music = xmas_victory_music;
            //music_as.volume = 0.15f;
        }
        else
        {
            currently_selected_menu_music = menu_music;
            currently_selected_game_music = game_music;
            currently_selected_boss_music = boss_music;
            currently_selected_victory_music = victory_music;
        }
    }

    public static void UpdateMusicVolume()
    {
        music_as.volume = VolumeManager.music_volume;
    }
}
