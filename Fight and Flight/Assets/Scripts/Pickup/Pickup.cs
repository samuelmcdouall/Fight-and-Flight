using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float revolution_time = 1.0f;
    [SerializeField]
    PickupType pickup_type;
    int rare_value = 5;
    int common_value = 1;
    public AudioClip collect_reward_sfx;
    public AudioClip collect_ammo_sfx;

    // Update is called once per frame
    void Update()
    {
        if (pickup_type == PickupType.ammo)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 360.0f / revolution_time);
        }
        else
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * 360.0f / revolution_time);
        }
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            if (pickup_type == PickupType.rare)
            {
                AudioSource.PlayClipAtPoint(collect_reward_sfx, transform.position);
                Player.score += rare_value;
            }
            else if (pickup_type == PickupType.common)
            {
                AudioSource.PlayClipAtPoint(collect_reward_sfx, transform.position);
                Player.score += common_value;
            }
            else
            {
                AudioSource.PlayClipAtPoint(collect_ammo_sfx, transform.position);
                if (Gun.ammo + 5 > Gun.max_ammo)
                {
                    Gun.ammo = Gun.max_ammo;
                }
                else
                {
                    Gun.ammo += 5;
                }
            }
            Destroy(gameObject);
        }
    }
    enum PickupType{
        common,
        rare,
        ammo
    }
}
