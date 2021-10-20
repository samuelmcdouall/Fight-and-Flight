using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    // General
    public GameObject platform;

    // Spawning parameters
    [SerializeField]
    [Range(5.0f, 20.0f)]
    float maximum_horizontal_spawn_distance = 20.0f;
    [SerializeField]
    [Range(2.0f, 5.0f)]
    float minimum_vertical_spawn_distance = 3.0f;
    [SerializeField]
    [Range(6.0f, 15.0f)]
    float maximum_vertical_spawn_distance = 12.0f;
    [SerializeField]
    [Range(8.0f, 10.0f)]
    float minimum_spawn_platform_proximity_distance = 8.0f;
    [SerializeField]
    [Range(15.0f, 40.0f)]
    float maximum_spawn_platform_proximity_distance = 15.0f;
    [SerializeField]
    int starting_platforms = 2;

    void Start()
    {
        for(int platform = 0; platform < starting_platforms; platform++)
        {
            AttemptToSpawnPlatform();
        }

    }

    public void AttemptToSpawnPlatform()
    {
        while (!SpawnedPlatformSuccessfully());
    }

    bool SpawnedPlatformSuccessfully()
    {
        Vector3 random_chosen_new_starting_position = RandomlyDetermineSpawnPosition();
        Vector3 random_chosen_y_independent_new_starting_position = new Vector3(random_chosen_new_starting_position.x, 0.0f, random_chosen_new_starting_position.z);

        if (SpawnPositionAppropriate(random_chosen_y_independent_new_starting_position))
        {
            Instantiate(platform, random_chosen_new_starting_position, Quaternion.identity);
            return true;
        }
        return false;
    }

    Vector3 RandomlyDetermineSpawnPosition()
    {
        float random_chosen_x_position = Random.Range(-maximum_horizontal_spawn_distance, maximum_horizontal_spawn_distance);
        float random_chosen_y_position = Random.Range(minimum_vertical_spawn_distance, maximum_vertical_spawn_distance);
        float random_chosen_z_position = Random.Range(-maximum_horizontal_spawn_distance, maximum_horizontal_spawn_distance);
        return new Vector3(random_chosen_x_position, random_chosen_y_position, random_chosen_z_position);
    }


    bool SpawnPositionAppropriate(Vector3 random_chosen_y_independent_new_starting_position)
    {
        bool spawned_close_enough_to_existing_platform = false;
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");

        foreach (GameObject platform in platforms)
        {
            Vector3 platform_y_independent_position = new Vector3(
                                                          platform.gameObject.transform.position.x,
                                                          0.0f,
                                                          platform.gameObject.transform.position.z
                                                      );
            if (Vector3.Distance(platform_y_independent_position, random_chosen_y_independent_new_starting_position) < minimum_spawn_platform_proximity_distance)
            {
                return false;
            }
            if (Vector3.Distance(platform_y_independent_position, random_chosen_y_independent_new_starting_position) < maximum_spawn_platform_proximity_distance)
            {
                spawned_close_enough_to_existing_platform = true;
            }
        }
        if (spawned_close_enough_to_existing_platform)
        {
            return true;
        }
        return false;
    }
}
