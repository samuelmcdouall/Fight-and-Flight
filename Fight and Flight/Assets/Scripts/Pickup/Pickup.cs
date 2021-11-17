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
        switch (pickup_type)
        {
            case PickupType.ammo:
            case PickupType.xmas_common:
            case PickupType.xmas_rare:
                transform.Rotate(Vector3.up, Time.deltaTime * 360.0f / revolution_time);
                break;
            case PickupType.common:
            case PickupType.rare:
                transform.Rotate(Vector3.forward, Time.deltaTime * 360.0f / revolution_time);
                break;
            default:
                print("defaulted, invalid value");
                break;
        }
    }

    void InitialPickupSetup()
    {
        rare_value = 3;
        common_value = 1;
        ammo_value = 5;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player") {
            switch (pickup_type)
            {
                case PickupType.rare:
                case PickupType.xmas_rare:
                    CollectReward(rare_value);
                    break;
                case PickupType.common:
                case PickupType.xmas_common:
                    CollectReward(common_value);
                    break;
                case PickupType.ammo:
                    AudioSource.PlayClipAtPoint(collect_ammo_sfx, transform.position, VolumeManager.sfx_volume);
                    CollectAmmo();
                    break;
                default:
                    print("defaulted, invalid value");
                    break;

            }
            Destroy(gameObject);
        }
    }

    void CollectReward(int value)
    {
        AudioSource.PlayClipAtPoint(collect_reward_sfx, transform.position, VolumeManager.sfx_volume);
        Player.score += value;
    }

    void CollectAmmo()
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
