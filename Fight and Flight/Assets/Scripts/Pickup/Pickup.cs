using UnityEngine;

public class Pickup : MonoBehaviour
{
    // General
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float revolution_time;

    // Collecting
    [SerializeField]
    PickupType pickup_type;
    int rare_value;
    int common_value;
    int ammo_value;
    public AudioClip collect_reward_sfx;
    public AudioClip collect_ammo_sfx;

    void Start()
    {
        InitialPickupSetup();
    }

    void Update()
    {
        if (pickup_type == PickupType.ammo || pickup_type == PickupType.xmas_common || pickup_type == PickupType.xmas_rare)
        {
            transform.Rotate(Vector3.up, Time.deltaTime * 360.0f / revolution_time);
        }
        else
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * 360.0f / revolution_time);
        }
    }

    private void InitialPickupSetup()
    {
        rare_value = 3;
        common_value = 1;
        ammo_value = 5;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            if (pickup_type == PickupType.rare || pickup_type == PickupType.xmas_rare)
            {
                CollectReward(rare_value);
            }
            else if (pickup_type == PickupType.common || pickup_type == PickupType.xmas_common)
            {
                CollectReward(common_value);
            }
            else
            {
                AudioSource.PlayClipAtPoint(collect_ammo_sfx, transform.position);
                CollectAmmo();
            }
            Destroy(gameObject);
        }
    }

    private void CollectReward(int value)
    {
        AudioSource.PlayClipAtPoint(collect_reward_sfx, transform.position);
        Player.score += value;
    }

    private void CollectAmmo()
    {
        if (Gun.ammo + ammo_value > Gun.max_ammo)
        {
            Gun.ammo = Gun.max_ammo;
        }
        else
        {
            Gun.ammo += ammo_value;
        }
    }

    enum PickupType{
        common,
        rare,
        xmas_common,
        xmas_rare,
        ammo
    }
}
