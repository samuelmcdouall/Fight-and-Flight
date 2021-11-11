using System.Collections.Generic;
using UnityEngine;
using Unity.RemoteConfig;

public class DroneSpawner : MonoBehaviour
{
    // General
    public GameObject drone;
    public GameObject xmas_drone;
    GameObject currently_selected_drone;
    public GameObject advanced_drone;
    public GameObject xmas_advanced_drone;
    GameObject currently_selected_advanced_drone;
    public GameObject boss_drone;
    public GameObject xmas_boss_drone;
    GameObject currently_selected_boss_drone;
    GameObject player;
    float drone_spawn_difficulty_modifier;

    // Spawning parameters
    [SerializeField]
    [Range(5.0f, 20.0f)]
    float maximum_horizontal_spawn_distance;
    [SerializeField]
    [Range(12.0f, 15.0f)]
    float minimum_vertical_spawn_distance;
    [SerializeField]
    [Range(16.0f, 20.0f)]
    float maximum_vertical_spawn_distance;
    [SerializeField]
    [Range(8.0f, 10.0f)]
    float minimum_spawn_drone_proximity_distance;
    public AudioClip spawn_sfx;
    public AudioClip boss_spawn_sfx;
    float drone_spawn_interval;
    float elapsed_drone_spawn_timer;
    int drone_spawn_difficulty_setting;
    [SerializeField]
    int starting_drones;
    float audio_cue_distance;
    bool spawned_in_boss;
    [SerializeField]
    int chance_to_spawn_advanced_drone;

    // Waypoints
    public List<Transform> waypoints;
    public List<Transform> boss_spawn_waypoints;
    public int boss_spawn_location;
    public List<Transform> boss_waypoints;

    void Start()
    {      
        InitialDroneSpawnerSetup();
        for (int drone = 0; drone < starting_drones; drone++)
        {
            AttemptToSpawnDrone(DroneType.random);
        }

    }

    void Update()
    {
        AdjustForDifficultySetting();
        if (Player.score >= 50)
        {
            if (!spawned_in_boss)
            {
                DestroyAllCurrentDronesAndRockets();
                SpawnBossDrone();
                spawned_in_boss = true;
            }
        }
        else if (elapsed_drone_spawn_timer > drone_spawn_interval - drone_spawn_difficulty_setting * drone_spawn_difficulty_modifier)
        {
            AttemptToSpawnDrone(DroneType.random);
            elapsed_drone_spawn_timer = 0.0f;
        }
        else
        {
            elapsed_drone_spawn_timer += Time.deltaTime;
        }
    }

    private void AdjustForDifficultySetting()
    {
        switch (DifficultyManager.difficulty)
        {
            case DifficultyManager.Difficulty.easy:
                drone_spawn_difficulty_setting = Player.player_min_level;
                break;
            case DifficultyManager.Difficulty.normal:
                drone_spawn_difficulty_setting = Player.player_current_level;
                break;
            case DifficultyManager.Difficulty.hard:
                drone_spawn_difficulty_setting = Player.player_max_level;
                break;
            default:
                break;
        }
    }

    private void InitialDroneSpawnerSetup()
    {
        ChooseDrones();
        player = GameObject.FindGameObjectWithTag("Player");
        drone_spawn_interval = 20.0f;
        drone_spawn_difficulty_modifier = 2.0f;
        elapsed_drone_spawn_timer = 0.0f;
        audio_cue_distance = 2.0f;
        spawned_in_boss = false;
    }

    void ChooseDrones()
    {
        if (RemoteConfigSettings.instance.xmas)
        {
            currently_selected_drone = xmas_drone;
            currently_selected_advanced_drone = xmas_advanced_drone;
            currently_selected_boss_drone = xmas_boss_drone;
        }
        else
        {
            currently_selected_drone = drone;
            currently_selected_advanced_drone = advanced_drone;
            currently_selected_boss_drone = boss_drone;
        }
    }

    private void SpawnBossDrone()
    {
        boss_spawn_location = Random.Range(0, 4);
        Instantiate(currently_selected_boss_drone, boss_spawn_waypoints[boss_spawn_location].position, Quaternion.identity);
        Vector3 player_to_drone_direction = (boss_spawn_waypoints[boss_spawn_location].position - player.transform.position).normalized;
        Vector3 audio_cue_position = player.transform.position + player_to_drone_direction * audio_cue_distance;
        AudioSource.PlayClipAtPoint(boss_spawn_sfx, audio_cue_position, VolumeManager.sfx_volume);
    }

    private static void DestroyAllCurrentDronesAndRockets()
    {
        GameObject[] drones = GameObject.FindGameObjectsWithTag("Drone");
        foreach (GameObject drone in drones)
        {
            Destroy(drone);
        }
        GameObject[] advanced_drones = GameObject.FindGameObjectsWithTag("Advanced Drone");
        foreach (GameObject advanced_drone in advanced_drones)
        {
            Destroy(advanced_drone);
        }
        GameObject[] rockets = GameObject.FindGameObjectsWithTag("Drone Rocket");
        foreach (GameObject rocket in rockets)
        {
            Destroy(rocket);
        }
    }

    public void AttemptToSpawnDrone(DroneType drone_type)
    {
        while (!SpawnedDroneSuccessfully(drone_type));
    }

    bool SpawnedDroneSuccessfully(DroneType drone_type)
    {
        //if (!currently_selected_drone)
        //{
        //    return false;
        //}
        Vector3 random_chosen_new_starting_position = RandomlyDetermineSpawnPosition();
        Vector3 random_chosen_y_independent_new_starting_position = new Vector3(random_chosen_new_starting_position.x, 0.0f, random_chosen_new_starting_position.z);

        if (SpawnPositionTooCloseToOtherDrones(random_chosen_y_independent_new_starting_position))
        {
            return false;
        }

        SpawnDrone(random_chosen_new_starting_position, drone_type);
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

    private void SpawnDrone(Vector3 random_chosen_new_starting_position, DroneType drone_type)
    {
        if (drone_type == DroneType.random)
        {
            int r = Random.Range(1, 101);
            if (r <= chance_to_spawn_advanced_drone)
            {
                Instantiate(currently_selected_advanced_drone, random_chosen_new_starting_position, Quaternion.identity);
            }
            else
            {
                Instantiate(currently_selected_drone, random_chosen_new_starting_position, Quaternion.identity);
            }
        }
        else if (drone_type == DroneType.normal)
        {
            Instantiate(currently_selected_drone, random_chosen_new_starting_position, Quaternion.identity);
        }
        else
        {
            Instantiate(currently_selected_advanced_drone, random_chosen_new_starting_position, Quaternion.identity);
        }
        Vector3 player_to_drone_direction = (random_chosen_new_starting_position - player.transform.position).normalized;
        Vector3 audio_cue_position = player.transform.position + player_to_drone_direction * audio_cue_distance;
        AudioSource.PlayClipAtPoint(spawn_sfx, audio_cue_position, VolumeManager.sfx_volume);
    }

    public enum DroneType
    {
        normal,
        advanced,
        random
    }
}
