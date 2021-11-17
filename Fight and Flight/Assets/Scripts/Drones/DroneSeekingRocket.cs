using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSeekingRocket : DroneRocket
{
    [SerializeField]
    float seeking_rocket_speed;
    GameObject player_hit_box;
    Rigidbody drone_rocket_rb;
    void Start()
    {
        InitialDroneRocketSetup();
        player_hit_box = GameObject.FindGameObjectWithTag("Drone Target");
        drone_rocket_rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        transform.LookAt(player_hit_box.transform);
        Vector3 rocket_rotation_offset = new Vector3(90.0f, 0.0f, 0.0f);
        transform.Rotate(rocket_rotation_offset);
    }
    void FixedUpdate()
    {
        drone_rocket_rb.velocity = (player_hit_box.transform.position - transform.position).normalized * seeking_rocket_speed;
    }
}
