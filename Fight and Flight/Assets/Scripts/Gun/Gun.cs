using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public static int max_ammo = 10;
    public static int ammo = 0;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float ammo_speed = 5.0f;
    public bool gun_trigger_pressed = false;
    public GameObject gun_barrel_outer;
    public GameObject gun_barrel_inner;
    public GameObject gun_bolt;
    public AudioClip fire_sfx;
    Quaternion gun_ammo_offset = Quaternion.Euler(0.0f, 0.0f, -90.0f);
    public RightHandController right_hand_controller_script;
    public LeftHandController left_hand_controller_script;
    // Start is called before the first frame update
    void Start()
    {
        ammo = max_ammo;
    }

    // Update is called once per frame
    void Update()
    {
        if (gun_trigger_pressed)
        {
            if (!Player.game_over && !Player.paused)
            {
                if (ammo != 0)
                {
                    Quaternion rotation_spawn_offset = gun_barrel_outer.transform.rotation * gun_ammo_offset;
                    GameObject ammo_object = Instantiate(gun_bolt, gun_barrel_outer.transform.position, rotation_spawn_offset);
                    ammo_object.GetComponent<Rigidbody>().velocity = (gun_barrel_outer.transform.position - gun_barrel_inner.transform.position).normalized * ammo_speed;
                    AudioSource.PlayClipAtPoint(fire_sfx, transform.position);
                    ammo--;
                    gun_trigger_pressed = false;
                }
            }

            // FOR THE MOMENT THIS IS THE PLACE TO RELOAD THE SCENE will use a proper menu later
            else if (!Player.paused)
            {
                right_hand_controller_script.RemoveActions();
                left_hand_controller_script.RemoveActions();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
