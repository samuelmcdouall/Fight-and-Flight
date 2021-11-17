using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RemoteConfigSettings : MonoBehaviour
{
    public bool remote_config_settings_obtained;
    public bool xmas = false;
    bool loaded_menu = false;
    float loading_timer = 1.0f;

    public static RemoteConfigSettings instance;

    // Remote Config
    public struct user_attributes { }
    public struct app_attributes { }
    // Start is called before the first frame update
    void Awake()
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
        remote_config_settings_obtained = false;
        ConfigManager.FetchCompleted += ObtainRemoteConfigSettings;
        ConfigManager.FetchConfigs<user_attributes, app_attributes>(new user_attributes(), new app_attributes());
    }

    void ObtainRemoteConfigSettings(ConfigResponse response)
    {
        xmas = ConfigManager.appConfig.GetBool("xmas");
        remote_config_settings_obtained = true;
        if (xmas)
        {
            print("Christmas theme!");
        }
        else
        {
            print("Regular theme");
        }
    }
    void Update()
    {
        if (!loaded_menu && remote_config_settings_obtained)
        {
            loading_timer -= Time.deltaTime;
            if (loading_timer <= 0.0f)
            {
                loaded_menu = true;
                SceneManager.LoadScene("MenuScene");
            }
        }
    }

    void OnDestroy()
    {
        ConfigManager.FetchCompleted -= ObtainRemoteConfigSettings;
    }
}
