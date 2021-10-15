using UnityEngine;
using UnityEngine.SceneManagement;

public class GunBolt : MonoBehaviour
{
    [SerializeField]
    float lifetime = 10.0f;
    public GameObject explosion_fx;
    public AudioClip explosion_sfx;
    GameObject player;
    int drone_value = 3;
    int boss_drone_value = 10;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Platform")
        {
            Explode();
        }
        else if (collider.gameObject.tag == "Target")
        {
            Destroy(collider.gameObject);
            Explode();
        }
        else if (collider.gameObject.tag == "Drone Hit Box")
        {
            Player.score += drone_value;
            Player.drones_destroyed++;
            Destroy(collider.gameObject.transform.parent.gameObject);
            Explode();
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
                Player.score += boss_drone_value;
                Player.drones_destroyed++;
                Player.victory = true;
                Destroy(collider.gameObject.transform.parent.gameObject);
                Explode();
            }
            else
            {
                collider.gameObject.transform.parent.gameObject.GetComponent<BossDrone>().current_boss_drone_hp--;
                // set boss bar here
                Explode();
            }
        }
    }

    private void Explode()
    {
        AudioSource.PlayClipAtPoint(explosion_sfx, player.transform.position);
        Instantiate(explosion_fx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
