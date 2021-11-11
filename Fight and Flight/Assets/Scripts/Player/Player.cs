using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // General
    [SerializeField]
    bool test_boss_battle;
    public static bool in_menu;
    public static bool boss_spawned;
    public static bool victory_countdown_begun;
    float victory_countdown_timer;
    float y_out_of_bounds;

    // Movement Mechanics
    Rigidbody player_rb;
    public bool throttle;
    float current_throttle;
    Vector3 current_direction;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float fly_speed;
    [SerializeField]
    [Range(0.0f, 2.0f)]
    float glide_speed;

    // Fuel Mechanics
    GroundCheck ground_check;
    [SerializeField]
    [Range(0.0f, 500.0f)]
    float max_fuel;
    float fuel;
    public FuelGauge fuel_gauge;
    public FuelMeter fuel_meter;
    [SerializeField]
    [Range(0.0f, 3.0f)]
    float fuel_recharge_delay;
    [SerializeField]
    [Range(0.0f, 250.0f)]
    float fuel_recharge_rate;
    float elapsed_fuel_recharge_delay;

    // Audio
    AudioSource player_as;
    public AudioClip flying_sfx;
    public AudioClip game_over_sfx;

    // Progression
    public static int score;
    public static int player_current_level;
    public static int player_min_level;
    public static int player_max_level;
    public static int drones_destroyed;
    int level_increase_rate;

    // UI screens
    public static bool game_over;
    public static bool paused; 
    public static bool victory;
    public GameObject game_over_screen;
    public GameObject statistics;
    public GameObject pause_screen;
    public GameObject victory_screen;

    void Start()
    {
        InitialPlayerSetup();
    }
    void Update()
    {
        if (!victory && victory_countdown_begun)
        {
            if (victory_countdown_timer <= 0.0f)
            {
                victory = true;
            }
            else
            {
                victory_countdown_timer -= Time.deltaTime;
            }
        }
        if (victory)
        {
            if (victory_screen.activeSelf == false)
            {
                victory_screen.SetActive(true);
                statistics.SetActive(false);
                if (player_as.isPlaying)
                {
                    player_as.Stop();
                }
                Time.timeScale = 0.0f;
            }
        }
        else if (game_over)
        {
            if (game_over_screen.activeSelf == false)
            {
                EnableGameOverScreen();
            }
        }
        else if (paused)
        {
            if (pause_screen.activeSelf == false)
            {
                EnablePauseScreen();
            }
        }
        else
        {
            if (pause_screen.activeSelf == true)
            {
                DisablePauseScreen();
            }
            DetermineCurrentPlayerLevel();
            CheckIfOutOfBounds();
            CheckIfAbleToRechargeEnergy();
            DetermineFlightSpeedAndDirection();
        }
    }
    private void InitialPlayerSetup()
    {
        GetPlayerComponents();
        InitialFuelSetup();
        InitialProgressionSetup();
        InitialUISetup();
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            in_menu = true;
        }
        else
        {
            in_menu = false;
        }
        Time.timeScale = 1.0f;
    }

    private void GetPlayerComponents()
    {
        player_as = GetComponent<AudioSource>();
        player_rb = GetComponent<Rigidbody>();
        ground_check = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<GroundCheck>();
    }
    private void InitialFuelSetup()
    {
        fuel = max_fuel;
        fuel_gauge.SetMaxFuelGauge(max_fuel);
        fuel_meter.SetNonGlidingColour();
        elapsed_fuel_recharge_delay = 0.0f;
        throttle = false;
        current_throttle = 0.0f;
}
    private void InitialProgressionSetup()
    {
        boss_spawned = false;
        y_out_of_bounds = 0.5f;
        victory_countdown_begun = false;
        victory_countdown_timer = 2.0f;
        if (test_boss_battle)
        {
            score = 49;
        }
        else
        {
            score = 0;
        }
        player_current_level = 0;
        player_min_level = 0;
        player_max_level = 4;
        drones_destroyed = 0;
        level_increase_rate = 10;
}
    private static void InitialUISetup()
    {
        game_over = false;
        paused = false;
        victory = false;
    }
    private void EnableGameOverScreen()
    {
        game_over_screen.SetActive(true);
        if (statistics)
        {
            statistics.SetActive(false);
        }
        if (player_as.isPlaying)
        {
            player_as.Stop();
        }
        player_as.PlayOneShot(game_over_sfx, VolumeManager.sfx_volume);
        Time.timeScale = 0.0f;
    }
    private void EnablePauseScreen()
    {
        pause_screen.SetActive(true);
        statistics.SetActive(false);
        if (player_as.isPlaying)
        {
            player_as.Stop();
        }
        Time.timeScale = 0.0f;
    }
    private void DisablePauseScreen()
    {
        pause_screen.SetActive(false);
        statistics.SetActive(true);
        Time.timeScale = 1.0f;
    }
    private void DetermineCurrentPlayerLevel()
    {
        if (score / level_increase_rate <= 4)
        {
            player_current_level = score / level_increase_rate;
        }
        else
        {
            player_current_level = 4;
        }
    }
    private void CheckIfOutOfBounds()
    {
        if (transform.position.y <= y_out_of_bounds)
        {
            game_over = true;
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
    private void DetermineFlightSpeedAndDirection()
    {
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
        if (!player_as.isPlaying)
        {
            player_as.PlayOneShot(flying_sfx, VolumeManager.sfx_volume);
        }
    }
    private void Glide()
    {
        player_rb.velocity = new Vector3(current_direction.x * glide_speed, -1.0f, current_direction.z * glide_speed);
        fuel_meter.SetGlidingColour();
    }
    public void UpdateThrottleValue(float trigger_amount)
    {
        current_throttle = trigger_amount;
    }

    public void UpdateDirection(Vector3 direction)
    {
        current_direction = direction;
    }
    public void PauseUnpause()
    {
        paused = !paused;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Drone Hit Box" || collision.gameObject.tag == "Boss Drone Hit Box" || collision.gameObject.tag == "Advanced Drone Hit Box")
        {
            game_over = true;
        }
    }
}
