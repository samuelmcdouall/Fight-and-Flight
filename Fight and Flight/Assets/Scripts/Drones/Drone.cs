using System.Collections.Generic;
using UnityEngine;

public class Drone : DroneBase
{
    // General
    float drone_speed_difficulty_modifier;
    
    void Start()
    {
        InitialDroneSetup();
    }

    void Update()
    {
        transform.LookAt(player_hit_box.transform);
        if (!menu_drone)
        {
            DetermineWaypointAndMove();
            DetermineIfTimeToFire();
        }
    }

    public void InitialDroneSetup()
    {
        InitialBaseDroneSetup();
        waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().waypoints);
        drone_speed_difficulty_modifier = 1.0f;
        drone_speed = Player.player_level * drone_speed_difficulty_modifier;
        waypoint_determined = false;
    }
}
