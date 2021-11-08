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
    int boss_drone_value;
    RightHandController right_hand_controller;
    LeftHandController left_hand_controller;
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
        right_hand_controller = GameObject.FindGameObjectWithTag("Right Hand Controller").GetComponent<RightHandController>();
        left_hand_controller = GameObject.FindGameObjectWithTag("Left Hand Controller").GetComponent<LeftHandController>();
        drone_value = 5;
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
        else if (collider.gameObject.tag == "Menu Drone Hit Box")
        {
            right_hand_controller.RemoveActions();
            left_hand_controller.RemoveActions();
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
                collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp--;
                boss_healthbar.SetBossBar(collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp);
                Explode(explosion_fx);
            }
        }
    }

    private void Explode(GameObject explosion_fx)
    {
        AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position);
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
