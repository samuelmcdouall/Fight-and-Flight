using UnityEngine;

public class DroneRocket : MonoBehaviour
{
    [SerializeField]
    float lifetime;
    public GameObject explosion_fx;
    public AudioClip explosion_sfx;
    GameObject player;
    GameObject platform_spawner;
    void Start()
    {
        InitialDroneRocketSetup();
    }

    public void InitialDroneRocketSetup()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        platform_spawner = GameObject.FindGameObjectWithTag("Platform Spawner");
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Platform")
        {
            Explode();
            if (gameObject.tag == "Boss Drone Rocket")
            {
                platform_spawner.GetComponent<PlatformSpawner>().AttemptToSpawnPlatform();
                Destroy(collider.gameObject);
            }
        }
        else if (collider.gameObject.tag == "Player")
        {
            Player.game_over = true;
            HighScoreTracker.CheckAgainstCurrentHighScore();
        }
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position, VolumeManager.sfx_volume);
        Instantiate(explosion_fx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
