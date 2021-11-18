using UnityEngine;

public class Platform : MonoBehaviour
{
    // General
    public bool menu_platform = false;
    [SerializeField]
    bool starting_platform;

    // Platform Lifetime
    bool deteriorating;
    float max_life_time;
    float current_life_time;
    Material platform_m;
    float opacity_lerp_time;

    // Platform Spawning
    GameObject platform_spawner;
    float y_out_of_bounds;
    [SerializeField]
    int percentage_chance_to_spawn_common_pickup;
    [SerializeField]
    int percentage_chance_to_spawn_rare_pickup;
    [SerializeField]
    int percentage_chance_to_spawn_ammo_pickup;
    [SerializeField]
    int percentage_chance_to_spawn_common_pickup_boss;
    [SerializeField]
    int percentage_chance_to_spawn_rare_pickup_boss;
    [SerializeField]
    int percentage_chance_to_spawn_ammo_pickup_boss;
    float pickup_spawn_range_limit;
    public GameObject common_pickup;
    public GameObject rare_pickup;
    public GameObject ammo_pickup;
    public GameObject xmas_common_pickup;
    public GameObject xmas_rare_pickup;
    GameObject currently_selected_common_pickup;
    GameObject currently_selected_rare_pickup;
    bool destruction_begun;

    void Start()
    {
        if (!menu_platform)
        {
            InitialPlatformSetup();
            if (!starting_platform)
            {
                ChoosePickups();
                DetermineIfPickUpSpawnedOnPlatform();
            }
        }
    }

    void Update()
    {
        if (!menu_platform)
        {
            if (deteriorating)
            {
                if (current_life_time > 0.0f)
                {
                    current_life_time -= Time.deltaTime;
                    ChangePlatformOpacity();
                }
                else if (current_life_time < 0.0f)
                {
                    current_life_time = 0.0f;
                }
                else
                {
                    if (!destruction_begun)
                    {
                        ReplacePlatform();
                    }
                }
            }
        }
    }

    void DetermineIfPickUpSpawnedOnPlatform()
    {
        if (!PlayerPCTest.boss_spawned)
        {
            int random_spawn_chance = Random.Range(1, 101);
            if (random_spawn_chance < percentage_chance_to_spawn_common_pickup + 1)
            {
                SpawnPickUp(currently_selected_common_pickup);
            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup + percentage_chance_to_spawn_rare_pickup + 1)
            {
                SpawnPickUp(currently_selected_rare_pickup);
            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup + percentage_chance_to_spawn_rare_pickup + percentage_chance_to_spawn_ammo_pickup + 1)
            {
                SpawnPickUp(ammo_pickup);
            }
        }
        else
        {
            int random_spawn_chance = Random.Range(1, 101);
            if (random_spawn_chance < percentage_chance_to_spawn_common_pickup_boss + 1)
            {
                SpawnPickUp(currently_selected_common_pickup);
            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup_boss + percentage_chance_to_spawn_rare_pickup_boss + 1)
            {
                SpawnPickUp(currently_selected_rare_pickup);
            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup_boss + percentage_chance_to_spawn_rare_pickup_boss + percentage_chance_to_spawn_ammo_pickup_boss + 1)
            {
                SpawnPickUp(ammo_pickup);
            }
        }
    }

    void SpawnPickUp(GameObject pickup)
    {
        float pickup_random_x_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
        float pickup_random_z_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
        Vector3 pick_up_offset = transform.position + new Vector3(pickup_random_x_location, 1.0f, pickup_random_z_location);
        GameObject new_pickup = Instantiate(pickup, pick_up_offset, pickup.transform.rotation);
        new_pickup.transform.SetParent(gameObject.transform);
    }

    void InitialPlatformSetup()
    {
        max_life_time = 10.0f;
        deteriorating = false;
        opacity_lerp_time = 0.0f;
        y_out_of_bounds = 0.5f;
        pickup_spawn_range_limit = 2.0f;
        platform_m = GetComponent<Renderer>().material;
        platform_spawner = GameObject.FindGameObjectWithTag("Platform Spawner");
        destruction_begun = false;
    }

    void ChoosePickups()
    {
        if (RemoteConfigSettings.instance.xmas)
        {
            currently_selected_common_pickup = xmas_common_pickup;
            currently_selected_rare_pickup = xmas_rare_pickup;
        }
        else
        {
            currently_selected_common_pickup = common_pickup;
            currently_selected_rare_pickup = rare_pickup;
        }
    }

    void ReplacePlatform()
    {
        platform_spawner.GetComponent<PlatformSpawner>().AttemptToSpawnPlatform();
        DestroyPlatform();
    }

    void DestroyPlatform()
    {
        // Work around for issue with OnCollisionExit not working when object is destroyed
        // Moving it just before it is destroyed (and destroying any pickup on it)
        float offset_trigger_collision_exit = 20.0f;
        transform.position = new Vector3(transform.position.x, transform.position.y - offset_trigger_collision_exit, transform.position.z);
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        destruction_begun = true;
        Destroy(gameObject, 0.1f);
    }

    void ChangePlatformOpacity()
    {
        float current_opacity = Mathf.Lerp(1.0f, 0.0f, opacity_lerp_time / max_life_time);
        Color current_color = new Color(platform_m.color.r, platform_m.color.g, platform_m.color.b, current_opacity);
        platform_m.SetColor("_Color", current_color);
        opacity_lerp_time += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!deteriorating)
            {
                AdjustForDifficultySetting();
                current_life_time = max_life_time;
                deteriorating = true;
            }
        }
    }

    void AdjustForDifficultySetting()
    {
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                max_life_time -= PlayerPCTest.player_min_level;
                break;
            case DifficultyManager.Difficulty.normal:
                max_life_time -= PlayerPCTest.player_current_level;
                break;
            case DifficultyManager.Difficulty.hard:
                max_life_time -= PlayerPCTest.player_max_level;
                break;
            default:
                print("defaulted, invalid value");
                max_life_time -= PlayerPCTest.player_min_level;
                break;
        }
    }
}
