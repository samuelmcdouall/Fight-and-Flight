using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject red_spaceship;
    public GameObject blue_spaceship;
    [SerializeField]
    float spawn_min_timer;
    [SerializeField]
    float spawn_max_timer;
    float respawn_rate;
    float elapsed_timer = 0;
    void Start()
    {
        respawn_rate = Random.Range(spawn_min_timer, spawn_max_timer);
    }

    // Update is called once per frame
    void Update()
    {
        if (elapsed_timer > respawn_rate) {
            SpawnSpaceship();
            elapsed_timer = 0;
            respawn_rate = Random.Range(spawn_min_timer, spawn_max_timer);
        }
        else
        {
            elapsed_timer += Time.deltaTime;
        }
    }

    void SpawnSpaceship()
    {
        int randomly_select_spaceship = Random.Range(0, 2);
        if (randomly_select_spaceship == 0)
        {
            Instantiate(red_spaceship, transform.position, red_spaceship.transform.rotation);
        }
        else
        {
            Instantiate(blue_spaceship, transform.position, blue_spaceship.transform.rotation);
        }
    }
}
