using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBase : MonoBehaviour
{
    // General
    public GameObject player_hit_box;
    public GameObject drone_spawner;
    public float drone_speed;
    public bool menu_drone = false;

    // Firing 
    [SerializeField]
    public float rocket_speed;
    public float fire_interval;
    public float elapsed_fire_timer;
    public GameObject rocket_spawn_right;
    public GameObject rocket_spawn_left;
    public GameObject rocket;
    public AudioClip fire_sfx;
    public Vector3 rocket_rotation_offset;

    // Waypoint traversing
    public List<Transform> waypoint_targets;
    public float waypoint_threshold;
    public bool waypoint_determined;
    public int current_waypoint_target_num;
    public Transform current_waypoint_target;

    public virtual void InitialDroneSetup() 
    {
        return;
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
        if (elapsed_fire_timer > fire_interval - Player.player_level)
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
