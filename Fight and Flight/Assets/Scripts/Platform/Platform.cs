using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    bool deteriorating = false;
    public float max_life_time = 10.0f;
    float current_life_time;
    public float platform_fall_speed = 3.0f;
    Material platform_m;
    public Color color_max_life;
    public Color color_min_life;
    float color_lerp_time = 0;
    GameObject platform_spawner;
    float y_out_of_bounds = 0.5f;
    [SerializeField]
    int percentage_chance_to_spawn_common_pickup = 50;
    [SerializeField]
    int percentage_chance_to_spawn_rare_pickup = 25;
    [SerializeField]
    float pickup_spawn_range_limit = 2.0f;
    public GameObject common_pickup;
    public GameObject rare_pickup;
    [SerializeField]
    bool starting_platform = false;


    // Start is called before the first frame update

    void Start()
    {
        if (!starting_platform)
        {
            int random_spawn_chance = Random.Range(1, 101);
            if (random_spawn_chance < percentage_chance_to_spawn_common_pickup + 1)
            {
                float pickup_random_x_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
                float pickup_random_z_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
                Vector3 pick_up_offset = transform.position + new Vector3(pickup_random_x_location, 1.0f, pickup_random_z_location);
                GameObject new_common_pickup = Instantiate(common_pickup, pick_up_offset, common_pickup.transform.rotation);
                new_common_pickup.transform.SetParent(gameObject.transform);

            }
            else if (random_spawn_chance < percentage_chance_to_spawn_common_pickup + percentage_chance_to_spawn_rare_pickup + 1)
            {
                float pickup_random_x_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
                float pickup_random_z_location = Random.Range(-pickup_spawn_range_limit, pickup_spawn_range_limit);
                Vector3 pick_up_offset = transform.position + new Vector3(pickup_random_x_location, 1.0f, pickup_random_z_location);
                GameObject new_rare_pickup = Instantiate(rare_pickup, pick_up_offset, rare_pickup.transform.rotation);
                new_rare_pickup.transform.SetParent(gameObject.transform);
            }
        }
        current_life_time = max_life_time;
        platform_m = GetComponent<Renderer>().material;
        platform_spawner = GameObject.FindGameObjectWithTag("Platform Spawner");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= y_out_of_bounds)
        {
            platform_spawner.GetComponent<PlatformSpawner>().AttemptToSpawnPlatform();
            Destroy(gameObject);
        }
        if (deteriorating)
        {   
            if (current_life_time > 0.0f)
            {
                current_life_time -= Time.deltaTime;
                platform_m.SetColor("_Color", Color.Lerp(color_max_life, color_min_life, color_lerp_time/max_life_time));
                color_lerp_time += Time.deltaTime;
            }

            else if (current_life_time < 0.0f)
            {
                current_life_time = 0.0f;
            }
            
            else
            {
                transform.Translate(-Vector3.up * Time.deltaTime * platform_fall_speed);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            deteriorating = true;
        }
    }
}
