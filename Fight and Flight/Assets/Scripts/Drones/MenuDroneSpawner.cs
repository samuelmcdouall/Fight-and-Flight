using System.Collections;
using System.Collections.Generic;
using Unity.RemoteConfig;
using UnityEngine;

public class MenuDroneSpawner : MonoBehaviour
{
    // General
    public GameObject menu_drone;
    public GameObject xmas_menu_drone;
    GameObject currently_selected_menu_drone;

    // Remote Config
    public struct user_attributes { }
    public struct app_attributes { }

    void Start()
    {
        ConfigManager.FetchCompleted += ChooseDrones;
        ConfigManager.FetchConfigs<user_attributes, app_attributes>(new user_attributes(), new app_attributes());
    }

    void ChooseDrones(ConfigResponse response)
    {
        bool xmas = ConfigManager.appConfig.GetBool("xmas");
        if (xmas)
        {
            print("Christmas Theme!");
            currently_selected_menu_drone = xmas_menu_drone;
        }
        else
        {
            print("Regular Theme");
            currently_selected_menu_drone = menu_drone;
        }

        Instantiate(currently_selected_menu_drone, transform.position, transform.rotation);
    }

    private void OnDestroy()
    {
        ConfigManager.FetchCompleted -= ChooseDrones;
    }
}
