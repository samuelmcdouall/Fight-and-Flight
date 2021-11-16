using System.Collections.Generic;
using UnityEngine;

public class BossDrone : DroneBase
{
    // General
    BossHealthBarUI boss_healthbar_UI;
    [System.NonSerialized]
    public bool invulnerable;
    BossState boss_state;
    bool transitioned_to_fighting_state;
    bool transitioned_to_invulnerable_state;
    [SerializeField]
    int drones_to_spawn_first_invul_stage;
    [SerializeField]
    int drones_to_spawn_second_invul_stage;
    bool transitioned_to_first_invul_state;
    bool transitioned_to_second_invul_state;

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
        if (boss_state == BossState.fighting)
        {
            FightingState();
        }
        else
        {
            InvulnerableState();
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
        boss_healthbar_UI.SetMaxBossBar(current_drone_hp);
        invulnerable = false;
        boss_state = BossState.fighting;
        transitioned_to_fighting_state = false;
        transitioned_to_invulnerable_state = false;
        transitioned_to_first_invul_state = false;
        transitioned_to_second_invul_state = false;
        Player.boss_spawned = true;

    }

    private void FightingState()
    {
        if (!transitioned_to_fighting_state)
        {
            EnterFightingState();
        }

        DetermineCurrentSpeed();
        DetermineWaypointAndMove();
        DetermineIfTimeToFire();
        MoveToInvulnerableStateIfHPThresholdReached();
    }

    private void EnterFightingState()
    {
        boss_healthbar_UI.SetBossBar(current_drone_hp);
        invulnerable = false;
        transitioned_to_fighting_state = true;
    }

    private void DetermineCurrentSpeed()
    {
        if (arrived_at_inner_waypoints)
        {
            drone_speed = (15 - current_drone_hp) / 3;
        }
        else
        {
            drone_speed = 3.0f;
        }
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
        if (elapsed_fire_timer > fire_interval - (15 - current_drone_hp) / 6)
        {
            FireRocket(rocket, fire_rocket_sfx);
            elapsed_fire_timer = 0.0f;
        }
        else
        {
            elapsed_fire_timer += Time.deltaTime;
        }
    }

    private void MoveToInvulnerableStateIfHPThresholdReached()
    {
        if (current_drone_hp == 10 && !transitioned_to_first_invul_state || current_drone_hp == 5 && !transitioned_to_second_invul_state)
        {
            if (current_drone_hp == 10)
            {
                transitioned_to_first_invul_state = true;
            }
            else
            {
                transitioned_to_second_invul_state = true;
            }
            boss_state = BossState.invulnerable;
            transitioned_to_fighting_state = false;
        }
    }

    private void InvulnerableState()
    {
        if (!transitioned_to_invulnerable_state)
        {
            EnterInvulnerableState();
        }

        MoveToFightingStateIfNoDronesRemaining();
    }

    private void EnterInvulnerableState()
    {
        drone_speed = 0.0f;
        boss_healthbar_UI.SetBossBarInvulnerable(current_drone_hp);
        invulnerable = true;
        int drones_to_spawn;
        if (current_drone_hp == 10)
        {
            drones_to_spawn = drones_to_spawn_first_invul_stage;

            for (int drone_count = 0; drone_count < drones_to_spawn; drone_count++)
            {
                drone_spawner.GetComponent<DroneSpawner>().AttemptToSpawnDrone(DroneSpawner.DroneType.normal);
            }
        }
        else
        {
            drones_to_spawn = drones_to_spawn_second_invul_stage;

            for (int drone_count = 0; drone_count < drones_to_spawn; drone_count++)
            {
                drone_spawner.GetComponent<DroneSpawner>().AttemptToSpawnDrone(DroneSpawner.DroneType.advanced);
            }
        }
        transitioned_to_invulnerable_state = true;
    }

    private void MoveToFightingStateIfNoDronesRemaining()
    {
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        GameObject[] advanced_drones = GameObject.FindGameObjectsWithTag("Advanced Drone");
        if (drones.Length == 0 && advanced_drones.Length == 0)
        {
            boss_state = BossState.fighting;
            transitioned_to_invulnerable_state = false;
        }
    }

    enum BossState
    {
        fighting,
        invulnerable
    }
}
