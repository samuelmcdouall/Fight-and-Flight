using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBase : MonoBehaviour
{
    // General
    protected GameObject player_hit_box;
    protected GameObject drone_spawner;
    protected float drone_speed;
    public bool menu_drone = false;

    // Firing
    protected float rocket_speed;
    protected float fire_interval;
    protected float elapsed_fire_timer;
    public GameObject rocket_spawn_right;
    public GameObject rocket_spawn_left;
    public GameObject rocket;
    public AudioClip fire_sfx;
    protected Vector3 rocket_rotation_offset;

    // Waypoint traversing
    protected List<Transform> waypoint_targets;
    protected float waypoint_threshold;
    protected bool waypoint_determined;
    protected int current_waypoint_target_num;
    protected Transform current_waypoint_target;

    public void InitialBaseDroneSetup()
    {
        player_hit_box = GameObject.FindGameObjectWithTag("Drone Target");
        if (!menu_drone)
        {
            drone_spawner = GameObject.FindGameObjectWithTag("Drone Spawner");
            waypoint_targets = new List<Transform>();
            waypoint_threshold = 0.1f;
            rocket_speed = 10.0f;
            fire_interval = 5.0f;
            elapsed_fire_timer = 0.0f;
            rocket_rotation_offset = new Vector3(90.0f, 0.0f, 0.0f);
        }
    }

    public virtual void DetermineWaypointAndMove()
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
    public virtual void DetermineIfTimeToFire()
    {
        if (elapsed_fire_timer > fire_interval)
        {
            FireRocket();
            elapsed_fire_timer = 0.0f;
        }
        else
        {
            elapsed_fire_timer += Time.deltaTime;
        }
    }
    public void FireRocket()
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
