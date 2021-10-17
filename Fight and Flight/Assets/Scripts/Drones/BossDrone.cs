using System.Collections.Generic;
using UnityEngine;

public class BossDrone : MonoBehaviour
{
    // General
    GameObject player_hit_box;
    GameObject drone_spawner;
    BossHealthBarUI boss_healthbar_UI;
    float drone_speed = 0.0f;
    public int max_boss_drone_hp = 15;
    public int current_boss_drone_hp = 15;

    // Firing 
    [SerializeField]
    float rocket_speed = 10.0f;
    float fire_interval = 5.0f;
    float elapsed_fire_timer = 0.0f;
    public GameObject rocket_spawn_right;
    public GameObject rocket_spawn_left;
    public GameObject rocket;
    public AudioClip fire_sfx;
    Vector3 rocket_rotation_offset = new Vector3(90.0f, 0.0f, 0.0f);

    // Waypoint traversing
    public List<Transform> waypoint_targets;
    float waypoint_threshold = 0.1f;
    bool waypoint_determined = false;
    int current_waypoint_target_num;
    Transform current_waypoint_target;
    bool arrived_at_inner_waypoints = false;
    void Start()
    {
        player_hit_box = GameObject.FindGameObjectWithTag("Drone Target");
        boss_healthbar_UI = GameObject.FindGameObjectWithTag("Boss Drone Health Bar").GetComponent<BossHealthBarUI>();
        InitialDroneSetup();
    }

    void Update()
    {
        transform.LookAt(player_hit_box.transform);
        if (arrived_at_inner_waypoints)
        {
            drone_speed = (15 - current_boss_drone_hp)/3;
        }
        DetermineWaypointAndMove();
        DetermineIfTimeToFire();
    }

    private void InitialDroneSetup()
    {
        drone_spawner = GameObject.FindGameObjectWithTag("Drone Spawner");
        waypoint_targets = new List<Transform>();
        waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().boss_waypoints);
        current_waypoint_target = waypoint_targets[drone_spawner.GetComponent<DroneSpawner>().boss_spawn_location];
        drone_speed = 3.0f;
        current_boss_drone_hp = max_boss_drone_hp;
        waypoint_determined = true;
        boss_healthbar_UI.SetMaxBossBar(current_boss_drone_hp);
        Player.boss_spawned = true;

    }

    private void DetermineWaypointAndMove()
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
    private void DetermineIfTimeToFire()
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
    private void FireRocket()
    {
        int randomly_selected_rocket_rack = Random.Range(0, 2);
        if (randomly_selected_rocket_rack == 0)
        {
            GameObject rocket_object = Instantiate(rocket, rocket_spawn_right.transform.position, transform.rotation);
            rocket_object.transform.Rotate(rocket_rotation_offset);
            rocket_object.GetComponent<Rigidbody>().velocity = (player_hit_box.transform.position - rocket_spawn_right.transform.position).normalized * rocket_speed;
            AudioSource.PlayClipAtPoint(fire_sfx, player_hit_box.transform.position);
        }
        else
        {
            GameObject rocket_object = Instantiate(rocket, rocket_spawn_left.transform.position, transform.rotation);
            rocket_object.transform.Rotate(rocket_rotation_offset);
            rocket_object.GetComponent<Rigidbody>().velocity = (player_hit_box.transform.position - rocket_spawn_left.transform.position).normalized * rocket_speed;
            AudioSource.PlayClipAtPoint(fire_sfx, player_hit_box.transform.position);
        }
    }
}
