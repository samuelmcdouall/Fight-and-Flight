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
            drone_speed = Player.player_level;
            DetermineWaypointAndMove();
            DetermineIfTimeToFire();
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
