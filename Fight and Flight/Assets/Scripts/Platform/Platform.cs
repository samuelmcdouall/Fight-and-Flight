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
    float platform_fall_speed;
    Material platform_m;
    float transparency_lerp_time;
    bool begun_falling;

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
                    ChangePlatformColour();
                }
                else if (current_life_time < 0.0f)
                {
                    current_life_time = 0.0f;
                }
                else
                {
                    ReplacePlatform();
                }
            }
        }
    }

    private void DetermineIfPickUpSpawnedOnPlatform()
    {
        if (!Player.boss_spawned)
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

    private void SpawnPickUp(GameObject pickup)
    {
        float pickup_random_x_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
        float pickup_random_z_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
        Vector3 pick_up_offset = transform.position + new Vector3(pickup_random_x_location, 1.0f, pickup_random_z_location);
        GameObject new_pickup = Instantiate(pickup, pick_up_offset, pickup.transform.rotation);
        new_pickup.transform.SetParent(gameObject.transform);
    }

    private void InitialPlatformSetup()
    {
        max_life_time = 10.0f;
        deteriorating = false;
        platform_fall_speed = 3.0f;
        transparency_lerp_time = 0.0f;
        begun_falling = false;
        y_out_of_bounds = 0.5f;
        pickup_spawn_range_limit = 2.0f;
        platform_m = GetComponent<Renderer>().material;
        platform_spawner = GameObject.FindGameObjectWithTag("Platform Spawner");
    }

    private void ChoosePickups()
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

    private void ReplacePlatform()
    {
        platform_spawner.GetComponent<PlatformSpawner>().AttemptToSpawnPlatform();
        Destroy(gameObject);
    }

    private void ChangePlatformColour()
    {
        float current_transparency = Mathf.Lerp(1.0f, 0.0f, transparency_lerp_time / max_life_time);
        Color current_color = new Color(platform_m.color.r, platform_m.color.g, platform_m.color.b, current_transparency);
        platform_m.SetColor("_Color", current_color);
        transparency_lerp_time += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
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

    private void AdjustForDifficultySetting()
    {
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                max_life_time -= Player.player_min_level;
                break;
            case DifficultyManager.Difficulty.normal:
                max_life_time -= Player.player_current_level;
                break;
            case DifficultyManager.Difficulty.hard:
                max_life_time -= Player.player_max_level;
                break;
            default:
                break;
        }
    }
}
