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
    public Color color_max_life;
    public Color color_min_life;
    float color_lerp_time;
    bool begun_falling;
    public AudioClip fall_sfx;

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

    void Start()
    {
        if (!menu_platform)
        {
            if (!starting_platform)
            {
                DetermineIfPickUpSpawnedOnPlatform();
            }
            InitialPlatformSetup();
        }
    }

    void Update()
    {
        if (!menu_platform)
        {
            if (transform.position.y <= y_out_of_bounds)
            {
                ReplacePlatform();
            }
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
                    PlatformFalls();
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
                SpawnPickUp(common_pickup);
            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup + percentage_chance_to_spawn_rare_pickup + 1)
            {
                SpawnPickUp(rare_pickup);
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
                SpawnPickUp(common_pickup);
            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup_boss + percentage_chance_to_spawn_rare_pickup_boss + 1)
            {
                SpawnPickUp(rare_pickup);
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
        color_lerp_time = 0.0f;
        begun_falling = false;
        y_out_of_bounds = 0.5f;
        pickup_spawn_range_limit = 2.0f;
        platform_m = GetComponent<Renderer>().material;
        platform_spawner = GameObject.FindGameObjectWithTag("Platform Spawner");
    }

    private void ReplacePlatform()
    {
        platform_spawner.GetComponent<PlatformSpawner>().AttemptToSpawnPlatform();
        Destroy(gameObject);
    }

    private void ChangePlatformColour()
    {
        platform_m.SetColor("_Color", Color.Lerp(color_max_life, color_min_life, color_lerp_time / max_life_time));
        color_lerp_time += Time.deltaTime;
    }

    private void PlatformFalls()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * platform_fall_speed);
        if (!begun_falling)
        {
            AudioSource.PlayClipAtPoint(fall_sfx, transform.position);
            begun_falling = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!deteriorating)
            {
                max_life_time -= Player.player_level;
                current_life_time = max_life_time;
                deteriorating = true;
            }
        }
    }
}
