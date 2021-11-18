using UnityEngine;

public class GunPCTest : MonoBehaviour
{
    // Ammo
    public static int max_ammo;
    public static int ammo;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float ammo_speed;

    // Firing Mechanics
    public bool gun_trigger_pressed;
    public GameObject gun_barrel_outer;
    public GameObject gun_barrel_inner;
    public GameObject gun_bolt;
    public AudioClip fire_sfx;
    Quaternion gun_ammo_offset;
    void Start()
    {
        InitialGunSetup();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!PlayerPCTest.game_over && !PlayerPCTest.paused)
            {
                if (ammo != 0)
                {
                    FireGunRocket();
                    if (!PlayerPCTest.in_menu)
                    {
                        ammo--;
                    }
                    gun_trigger_pressed = false;
                }
            }
        }
    }

    void InitialGunSetup()
    {
        max_ammo = 10;
        ammo = max_ammo;
        gun_ammo_offset = Quaternion.Euler(0.0f, 0.0f, -90.0f);
        gun_trigger_pressed = false;
    }

    void FireGunRocket()
    {
        Quaternion rotation_spawn_offset = gun_barrel_outer.transform.rotation * gun_ammo_offset;
        GameObject ammo_object = Instantiate(gun_bolt, gun_barrel_outer.transform.position, rotation_spawn_offset);
        ammo_object.GetComponent<Rigidbody>().velocity = (gun_barrel_outer.transform.position - gun_barrel_inner.transform.position).normalized * ammo_speed;
        AudioSource.PlayClipAtPoint(fire_sfx, transform.position, VolumeManager.sfx_volume);
    }
}
