using System.Collections.Generic;
using UnityEngine;

public class Drone : DroneBase
{
    float difficulty_adjusted_speed;
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

    private void AdjustForDifficultySetting()
    {
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                drone_speed = Player.player_min_level;
                break;
            case DifficultyManager.Difficulty.normal:
                drone_speed = Player.player_current_level;
                break;
            case DifficultyManager.Difficulty.hard:
                drone_speed = Player.player_max_level;
                break;
            default:
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
