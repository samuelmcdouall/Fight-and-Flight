using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float rocket_speed = 10.0f;
    [SerializeField]
    [Range(0.1f, 100.0f)]
    float fire_interval = 5.0f;
    float elapsed_fire_timer = 0.0f;
    public GameObject rocket_spawn_right;
    public GameObject rocket_spawn_left;
    public GameObject rocket;
    public AudioClip fire_sfx;
    Vector3 rocket_rotation_offset = new Vector3(90.0f, 0.0f, 0.0f);
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Drone Target");
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform);
        if (elapsed_fire_timer > fire_interval)
        {
            //Quaternion rotation_spawn_offset = gun_barrel_outer.transform.rotation * gun_ammo_offset;
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
            elapsed_fire_timer = 0.0f;
        }
        else
        {
            elapsed_fire_timer += Time.deltaTime;
        }
    }
}
