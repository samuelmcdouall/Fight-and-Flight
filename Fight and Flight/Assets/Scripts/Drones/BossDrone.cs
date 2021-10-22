using System.Collections.Generic;
using UnityEngine;

public class BossDrone : DroneBase
{
    // General
    BossHealthBarUI boss_healthbar_UI;
    public int max_boss_drone_hp = 15;
    public int current_boss_drone_hp = 15;

    // Waypoint traversing
    bool arrived_at_inner_waypoints = false;

    void Start()
    {
        InitialDroneSetup();
    }

    void Update()
    {
        transform.LookAt(player_hit_box.transform);
        DetermineCurrentSpeed();
        DetermineWaypointAndMove();
        DetermineIfTimeToFire();
    }

    private void DetermineCurrentSpeed()
    {
        if (arrived_at_inner_waypoints)
        {
            drone_speed = (15 - current_boss_drone_hp) / 3;
        }
    }

    public override void InitialDroneSetup()
    {
        player_hit_box = GameObject.FindGameObjectWithTag("Drone Target");
        drone_spawner = GameObject.FindGameObjectWithTag("Drone Spawner");
        boss_healthbar_UI = GameObject.FindGameObjectWithTag("Boss Drone Health Bar").GetComponent<BossHealthBarUI>();
        waypoint_targets = new List<Transform>();
        waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().boss_waypoints);
        current_waypoint_target = waypoint_targets[drone_spawner.GetComponent<DroneSpawner>().boss_spawn_location];
        drone_speed = 3.0f;
        rocket_speed = 10.0f;
        fire_interval = 5.0f;
        elapsed_fire_timer = 0.0f;
        rocket_rotation_offset = new Vector3(90.0f, 0.0f, 0.0f);
        current_boss_drone_hp = max_boss_drone_hp;
        waypoint_determined = true;
        boss_healthbar_UI.SetMaxBossBar(current_boss_drone_hp);
        Player.boss_spawned = true;
        waypoint_threshold = 0.1f;
        waypoint_determined = false;

}

    public override void DetermineWaypointAndMove()
    {
        if (!waypoint_determined)
        {
            current_waypoint_target_num = Random.Range(0, waypoint_targets.Count);
            current_waypoint_target = waypoint_targets[current_waypoint_target_num];
            waypoint_determined = true;
        }

        if (Vector3.Distance(transform.position, current_waypoint_target.position) <= waypoint_threshold)
        {
            waypoint_determined = false;
            if (!arrived_at_inner_waypoints)
            {
                arrived_at_inner_waypoints = true;
            }
        }

        else
        {
            Vector3 drone_to_waypoint_target_direction = (current_waypoint_target.position - transform.position).normalized;
            transform.position += drone_to_waypoint_target_direction * Time.deltaTime * drone_speed;
        }
    }
    public override void DetermineIfTimeToFire()
    {
        if (elapsed_fire_timer > fire_interval - (15 - current_boss_drone_hp)/6)
        {
            FireRocket();
            elapsed_fire_timer = 0.0f;
        }
        else
        {
            elapsed_fire_timer += Time.deltaTime;
        }
    }
}
