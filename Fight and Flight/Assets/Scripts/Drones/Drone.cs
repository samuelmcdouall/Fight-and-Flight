using System.Collections.Generic;
using UnityEngine;

public class Drone : DroneBase
{
    void Start()
    {
        InitialDroneSetup();
    }

    void Update()
    {
        transform.LookAt(player_hit_box.transform);
        if (!menu_drone)
        {
            AdjustForDifficultySetting();
            DetermineWaypointAndMove();
            DetermineIfTimeToFire();
        }
    }

    void AdjustForDifficultySetting()
    {
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                drone_speed = PlayerPCTest.player_min_level;
                break;
            case DifficultyManager.Difficulty.normal:
                drone_speed = PlayerPCTest.player_current_level;
                break;
            case DifficultyManager.Difficulty.hard:
                drone_speed = PlayerPCTest.player_max_level;
                break;
            default:
                print("defaulted, invalid value");
                drone_speed = PlayerPCTest.player_min_level;
                break;
        }
    }

    public void InitialDroneSetup()
    {
        
        InitialBaseDroneSetup();
        if (!menu_drone)
        {
            waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().waypoints);
            waypoint_determined = false;
        }
    }
}
