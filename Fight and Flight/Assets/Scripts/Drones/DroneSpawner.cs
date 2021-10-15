using System.Collections.Generic;
using UnityEngine;

public class DroneSpawner : MonoBehaviour
{
    // General
    public GameObject drone;
    GameObject player;
    float drone_spawn_difficulty_modifier = 2.0f;

    // Spawning parameters
    [SerializeField]
    [Range(5.0f, 20.0f)]
    float maximum_horizontal_spawn_distance = 20.0f;
    [SerializeField]
    [Range(12.0f, 15.0f)]
    float minimum_vertical_spawn_distance = 15.0f;
    [SerializeField]
    [Range(16.0f, 20.0f)]
    float maximum_vertical_spawn_distance = 17.0f;
    [SerializeField]
    [Range(8.0f, 10.0f)]
    float minimum_spawn_drone_proximity_distance = 8.0f;
    public AudioClip spawn_sfx;
    float drone_spawn_interval = 20.0f;
    float elapsed_drone_spawn_timer = 0.0f;
    [SerializeField]
    int starting_drones = 2;
    float audio_cue_distance = 2.0f;

    // Waypoints
    public List<Transform> waypoints;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        for (int drone = 0; drone < starting_drones; drone++)
        {
            AttemptToSpawnDrone();
        }

    }
    void Update()
    {
        if (elapsed_drone_spawn_timer > drone_spawn_interval - Player.player_level * drone_spawn_difficulty_modifier)
        {
            AttemptToSpawnDrone();
            elapsed_drone_spawn_timer = 0.0f;
        }
        else
        {
            elapsed_drone_spawn_timer += Time.deltaTime;
        }
    }

    public void AttemptToSpawnDrone()
    {
        while (!SpawnedDroneSuccessfully());
    }

    bool SpawnedDroneSuccessfully()
    {
        Vector3 random_chosen_new_starting_position = RandomlyDetermineSpawnPosition();
        Vector3 random_chosen_y_independent_new_starting_position = new Vector3(random_chosen_new_starting_position.x, 0.0f, random_chosen_new_starting_position.z);

        if (SpawnPositionTooCloseToOtherDrones(random_chosen_y_independent_new_starting_position))
        {
            return false;
        }

        SpawnDrone(random_chosen_new_starting_position);
        return true;
    }

    Vector3 RandomlyDetermineSpawnPosition()
    {
        float random_chosen_x_position = Random.Range(-maximum_horizontal_spawn_distance, maximum_horizontal_spawn_distance);
        float random_chosen_y_position = Random.Range(minimum_vertical_spawn_distance, maximum_vertical_spawn_distance);
        float random_chosen_z_position = Random.Range(-maximum_horizontal_spawn_distance, maximum_horizontal_spawn_distance);
        return new Vector3(random_chosen_x_position, random_chosen_y_position, random_chosen_z_position);
    }

    bool SpawnPositionTooCloseToOtherDrones(Vector3 random_chosen_y_independent_new_starting_position)
    {
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject drone in drones)
        {
            Vector3 drone_y_independent_position = new Vector3(
                                                          drone.gameObject.transform.position.x,
                                                          0.0f,
                                                          drone.gameObject.transform.position.z
                                                      );
            if (Vector3.Distance(drone_y_independent_position, random_chosen_y_independent_new_starting_position) < minimum_spawn_drone_proximity_distance)
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnDrone(Vector3 random_chosen_new_starting_position)
    {
        Instantiate(drone, random_chosen_new_starting_position, Quaternion.identity);
        Vector3 player_to_drone_direction = (random_chosen_new_starting_position - player.transform.position).normalized;
        Vector3 audio_cue_position = player.transform.position + player_to_drone_direction * audio_cue_distance;
        AudioSource.PlayClipAtPoint(spawn_sfx, audio_cue_position);
    }
}
