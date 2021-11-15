using UnityEngine;
using UnityEngine.SceneManagement;

public class GunBolt : MonoBehaviour
{
    [SerializeField]
    float lifetime;
    public GameObject explosion_fx;
    public GameObject boss_explosion_fx;
    public AudioClip explosion_sfx;
    GameObject player;
    int drone_value;
    int advanced_drone_value;
    int boss_drone_value;
    BossHealthBarUI boss_healthbar;
    void Start()
    {
        InitialGunBoltSetup();
        if (Player.boss_spawned)
        {
            boss_healthbar = GameObject.FindGameObjectWithTag("Boss Drone Health Bar").GetComponent<BossHealthBarUI>();
        }
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (Player.boss_spawned && boss_healthbar == null)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss Drone Health Bar");
            if (boss)
            {
                boss_healthbar = boss.GetComponent<BossHealthBarUI>();
            }
        }
    }

    private void InitialGunBoltSetup()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        drone_value = 5;
        advanced_drone_value = 7;
        boss_drone_value = 10;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Platform")
        {
            Explode(explosion_fx);
        }
        else if (collider.gameObject.tag == "Target")
        {
            Destroy(collider.gameObject);
            Explode(explosion_fx);
        }
        else if (collider.gameObject.tag == "Drone Hit Box")
        {
            Player.score += drone_value;
            Player.drones_destroyed++;
            Destroy(collider.gameObject.transform.parent.gameObject);
            Explode(explosion_fx);
        }
        else if (collider.gameObject.tag == "Advanced Drone Hit Box")
        {
            if (collider.gameObject.transform.parent.gameObject.GetComponent<Drone>().current_drone_hp == 1)
            {
                Player.score += advanced_drone_value;
                Player.drones_destroyed++;
                Destroy(collider.gameObject.transform.parent.gameObject);
                Explode(explosion_fx);
            }
            else
            {
                collider.gameObject.transform.parent.gameObject.GetComponent<Drone>().current_drone_hp--;
                collider.gameObject.GetComponent<AdvancedDroneAnimation>().PlayDamageAnimation();
                Explode(explosion_fx);
            }
        }
        else if (collider.gameObject.tag == "Menu Drone Hit Box")
        {
            SceneManager.LoadScene("GameScene");
        }
        else if (collider.gameObject.tag == "Boss Drone Hit Box")
        {
            if (collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp == 1)
            {
                collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp--;
                boss_healthbar.SetBossBar(collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp);
                Player.score += boss_drone_value;
                Player.drones_destroyed++;
                Player.victory_countdown_begun = true;
                DestroyAllCurrentBossRockets();
                Destroy(collider.gameObject.transform.parent.gameObject);
                Explode(boss_explosion_fx);
            }
            else
            {
                if (!collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().invulnerable)
                {
                    collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp--;
                    boss_healthbar.SetBossBar(collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp);
                }
                Explode(explosion_fx);
            }
        }
        else if (collider.gameObject.tag == "Volume Control")
        {
            collider.gameObject.GetComponent<VolumeControl>().ChangeVolume();
            AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position, VolumeManager.sfx_volume);
            Destroy(gameObject);
        }
        else if (collider.gameObject.tag == "Difficulty Setting")
        {
            collider.gameObject.GetComponent<DifficultySetting>().ChangeDifficulty();
            AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position, VolumeManager.sfx_volume);
            Destroy(gameObject);
        }
    }

    private void Explode(GameObject explosion_fx)
    {
        AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position, VolumeManager.sfx_volume);
        Instantiate(explosion_fx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private static void DestroyAllCurrentBossRockets()
    {
        GameObject[] rockets = GameObject.FindGameObjectsWithTag("Boss Drone Rocket");
        foreach (GameObject rocket in rockets)
        {
            Destroy(rocket);
        }
    }
}
