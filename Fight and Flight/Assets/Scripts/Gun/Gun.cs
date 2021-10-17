using UnityEngine;

public class Gun : MonoBehaviour
{
    // Ammo
    public static int max_ammo = 10;
    public static int ammo = 0;
    [SerializeField]
    [Range(0.0f, 100.0f)]
    float ammo_speed = 5.0f;

    // Firing Mechanics
    public bool gun_trigger_pressed = false;
    public GameObject gun_barrel_outer;
    public GameObject gun_barrel_inner;
    public GameObject gun_bolt;
    public AudioClip fire_sfx;
    Quaternion gun_ammo_offset = Quaternion.Euler(0.0f, 0.0f, -90.0f);
    void Start()
    {
        ammo = max_ammo;
    }

    void Update()
    {
        if (gun_trigger_pressed)
        {
            if (!Player.game_over && !Player.paused)
            {
                if (ammo != 0)
                {
                    FireGunRocket();
                    if (!Player.in_menu)
                    {
                        ammo--;
                    }
                    gun_trigger_pressed = false;
                }
            }
        }
    }

    private void FireGunRocket()
    {
        Quaternion rotation_spawn_offset = gun_barrel_outer.transform.rotation * gun_ammo_offset;
        GameObject ammo_object = Instantiate(gun_bolt, gun_barrel_outer.transform.position, rotation_spawn_offset);
        ammo_object.GetComponent<Rigidbody>().velocity = (gun_barrel_outer.transform.position - gun_barrel_inner.transform.position).normalized * ammo_speed;
        AudioSource.PlayClipAtPoint(fire_sfx, transform.position);
    }
}
