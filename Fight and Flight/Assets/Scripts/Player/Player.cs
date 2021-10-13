using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool in_menu;
    Rigidbody player_rb;
    GroundCheck ground_check;
    public bool throttle = false;
    float current_throttle = 0.0f;
    Vector3 current_direction;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float fly_speed = 10.0f;
    [SerializeField]
    [Range(0.0f, 2.0f)]
    float glide_speed = 2.0f;
    [SerializeField]
    [Range(0.0f, 500.0f)]
    float max_fuel = 200.0f;
    float fuel;
    Vector3 starting_position;
    public FuelGauge fuel_gauge;
    public FuelMeter fuel_meter;
    [SerializeField]
    [Range(0.0f, 3.0f)]
    float fuel_recharge_delay = 1.0f;
    [SerializeField]
    [Range(0.0f, 250.0f)]
    float fuel_recharge_rate = 100.0f;
    float elapsed_fuel_recharge_delay = 0.0f;
    AudioSource player_as;
    public AudioClip flying_sfx;
    public AudioClip game_over_sfx;
    public AudioClip pause_sfx;
    public AudioClip unpause_sfx;
    public static int score = 0;
    public static int player_level = 0;
    public static int drones_destroyed = 0;
    int level_increase_rate = 10;
    float y_out_of_bounds = 0.5f;
    public static bool game_over = false;
    public static bool paused = false;
    public GameObject game_over_screen;
    public GameObject score_screen;
    public GameObject pause_screen;
    // Start is called before the first frame update
    void Start()
    {
        starting_position = transform.position;
        player_as = GetComponent<AudioSource>();
        player_rb = GetComponent<Rigidbody>();
        ground_check = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<GroundCheck>();
        fuel = max_fuel;
        fuel_gauge.SetMaxFuelGauge(max_fuel);
        fuel_meter.SetNonGlidingColour();
        score = 0;
        player_level = 0;
        drones_destroyed = 0;
        game_over = false;
        paused = false;
        Time.timeScale = 1.0f;
    }

    public void UpdateThrottleValue(float trigger_amount)
    {
        current_throttle = trigger_amount;
    }

    public void UpdateDirection(Vector3 direction)
    {
        current_direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        //print("elapsed fuel recharge delay: " + elapsed_fuel_recharge_delay);
        if (game_over)
        {
            if (game_over_screen.activeSelf == false)
            {
                game_over_screen.SetActive(true);
                score_screen.SetActive(false);
                if (player_as.isPlaying)
                {
                    player_as.Stop();
                }
                player_as.PlayOneShot(game_over_sfx, 1.0f);
                Time.timeScale = 0.0f;
            }
            return;
        }
        if (paused)
        {
            if (pause_screen.activeSelf == false)
            {
                pause_screen.SetActive(true);
                score_screen.SetActive(false);
                if (player_as.isPlaying)
                {
                    player_as.Stop();
                }
                //player_as.PlayOneShot(pause_sfx, 1.0f);
                Time.timeScale = 0.0f;
            }
        }
        else
        {
            if (pause_screen.activeSelf == true)
            {
                pause_screen.SetActive(false);
                score_screen.SetActive(true);
                //if (player_as.isPlaying)
                //{
                //    player_as.Stop();
                //}
                Time.timeScale = 1.0f;
                //hm seems to do it using playclipatpoint, not sure why
                //maybe dont need unpause sound
                //AudioSource.PlayClipAtPoint(unpause_sfx, transform.position);
            }
            if (score / level_increase_rate <= 4)
            {
                player_level = score / level_increase_rate;
            }
            else
            {
                player_level = 4;
            }
            CheckIfOutOfBounds();
            CheckIfAbleToRechargeEnergy();
            if (throttle && fuel != 0.0f)
            {
                Fly();
            }
            else if (throttle && fuel == 0.0f)
            {
                Glide();
                player_as.Stop();
            }
            else
            {
                fuel_meter.SetNonGlidingColour();
                player_as.Stop();
            }
        }
    }

    private void Glide()
    {
        player_rb.velocity = new Vector3(current_direction.x * glide_speed, -1.0f, current_direction.z * glide_speed);
        fuel_meter.SetGlidingColour();
    }

    private void Fly()
    {
        player_rb.velocity = current_direction * current_throttle * fly_speed;
        fuel -= current_throttle;
        if (fuel < 0.0f)
        {
            fuel = 0.0f;
        }
        fuel_gauge.SetFuelGauge(fuel);
        fuel_meter.SetNonGlidingColour();
        if (!player_as.isPlaying)// && !player_ani.GetCurrentAnimatorStateInfo(0).IsName("Light Attack"))
        {
            player_as.PlayOneShot(flying_sfx, 1.0f);
        }
    }

    private void CheckIfAbleToRechargeEnergy()
    {
        if (ground_check.is_grounded)
        {
            if (fuel != max_fuel)
            {
                if (elapsed_fuel_recharge_delay > fuel_recharge_delay)
                {
                    fuel += fuel_recharge_rate * Time.deltaTime;
                    if (fuel > max_fuel)
                    {
                        fuel = max_fuel;
                    }
                    fuel_gauge.SetFuelGauge(fuel);
                }
                else
                {
                    elapsed_fuel_recharge_delay += Time.deltaTime;
                }
            }
        }
        else
        {
            elapsed_fuel_recharge_delay = 0.0f;
        }
    }

    private void CheckIfOutOfBounds()
    {
        if (transform.position.y <= y_out_of_bounds)
        {
            game_over = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Drone Hit Box")
        {
            game_over = true;
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseUnpause()
    {
        paused = !paused;
    }
}
