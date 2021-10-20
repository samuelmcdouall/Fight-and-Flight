using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // General
    public bool menu_drone = false;
    GameObject player;
    GameObject drone_spawner;
    float drone_speed = 0.0f;
    float drone_speed_difficulty_modifier = 1.0f;
    float drone_fire_difficulty_modifier = 0.0f; // turn off for the moment might just remove this later
    
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
    List<Transform> waypoint_targets;
    float waypoint_threshold = 0.1f;
    bool waypoint_determined = false;
    int current_waypoint_target_num;
    Transform current_waypoint_target;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Drone Target");
        if (!menu_drone)
        {
            InitialDroneSetup();
        }
    }

    void Update()
    {
        transform.LookAt(player.transform);
        if (!menu_drone)
        {
            DetermineWaypointAndMove();
            DetermineIfTimeToFire();
        }
    }

    private void InitialDroneSetup()
    {
        drone_spawner = GameObject.FindGameObjectWithTag("Drone Spawner");
        waypoint_targets = new List<Transform>();
        waypoint_targets.AddRange(drone_spawner.GetComponent<DroneSpawner>().waypoints);
        drone_speed = Player.player_level * drone_speed_difficulty_modifier;
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
        }

        else
        {
            Vector3 drone_to_waypoint_target_direction = (current_waypoint_target.position - transform.position).normalized;
            transform.position += drone_to_waypoint_target_direction * Time.deltaTime * drone_speed;
        }
    }
    private void DetermineIfTimeToFire()
    {
        if (elapsed_fire_timer > fire_interval - Player.player_level * drone_fire_difficulty_modifier)
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
            rocket_object.GetComponent<Rigidbody>().velocity = (player.transform.position - rocket_spawn_right.transform.position).normalized * rocket_speed;
            AudioSource.PlayClipAtPoint(fire_sfx, player.transform.position);
        }
        else
        {
            GameObject rocket_object = Instantiate(rocket, rocket_spawn_left.transform.position, transform.rotation);
            rocket_object.transform.Rotate(rocket_rotation_offset);
            rocket_object.GetComponent<Rigidbody>().velocity = (player.transform.position - rocket_spawn_left.transform.position).normalized * rocket_speed;
            AudioSource.PlayClipAtPoint(fire_sfx, player.transform.position);
        }
    }
}
