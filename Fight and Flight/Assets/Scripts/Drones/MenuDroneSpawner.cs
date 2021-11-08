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
        ChooseMenuDrone();
    }

    void ChooseMenuDrone()
    {
        if (RemoteConfigSettings.instance.xmas)
        {
            currently_selected_menu_drone = xmas_menu_drone;
        }
        else
        {
            currently_selected_menu_drone = menu_drone;
        }

        Instantiate(currently_selected_menu_drone, transform.position, transform.rotation);
    }
}
