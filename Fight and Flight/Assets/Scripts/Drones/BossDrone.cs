using System.Collections.Generic;
using UnityEngine;

public class BossDrone : DroneBase
{
    // General
    BossHealthBarUI boss_healthbar_UI;
    [System.NonSerialized]
    public int max_boss_drone_hp;
    [System.NonSerialized]
    public int current_boss_drone_hp;

    // Waypoint traversing
    [System.NonSerialized]
    bool arrived_at_inner_waypoints;

    void Start()
    {
        InitialBossDroneSetup();
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

    public void InitialBossDroneSetup()
    {
        InitialBaseDroneSetup();
        waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().boss_waypoints);
        current_waypoint_target = waypoint_targets[drone_spawner.GetComponent<DroneSpawner>().boss_spawn_location];
        waypoint_determined = true;
        arrived_at_inner_waypoints = false;
        drone_speed = 3.0f;
        boss_healthbar_UI = GameObject.FindGameObjectWithTag("Boss Drone Health Bar").GetComponent<BossHealthBarUI>();
        max_boss_drone_hp = 15;
        current_boss_drone_hp = max_boss_drone_hp;
        boss_healthbar_UI.SetMaxBossBar(current_boss_drone_hp);
        Player.boss_spawned = true;

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
