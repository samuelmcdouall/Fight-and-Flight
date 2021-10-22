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

    public override void InitialDroneSetup()
    {
        player_hit_box = GameObject.FindGameObjectWithTag("Drone Target");
        if (!menu_drone)
        {
            drone_spawner = GameObject.FindGameObjectWithTag("Drone Spawner");
            waypoint_targets = new List<Transform>();
            waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().waypoints);
            drone_speed = Player.player_level * drone_speed_difficulty_modifier;
            drone_speed_difficulty_modifier = 1.0f;
            rocket_speed = 10.0f;
            fire_interval = 5.0f;
            elapsed_fire_timer = 0.0f;
            rocket_rotation_offset = new Vector3(90.0f, 0.0f, 0.0f);
            waypoint_threshold = 0.1f;
            waypoint_determined = false;
        }
    }
}
