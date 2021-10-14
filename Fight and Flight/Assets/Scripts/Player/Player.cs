using UnityEngine;

public class Player : MonoBehaviour
{
    // General
    public bool in_menu;
    float y_out_of_bounds = 0.5f;

    // Movement Mechanics
    Rigidbody player_rb;
    public bool throttle = false;
    float current_throttle = 0.0f;
    Vector3 current_direction;
    [SerializeField]
    [Range(0.0f, 10.0f)]
    float fly_speed = 10.0f;
    [SerializeField]
    [Range(0.0f, 2.0f)]
    float glide_speed = 2.0f;

    // Fuel Mechanics
    GroundCheck ground_check;
    [SerializeField]
    [Range(0.0f, 500.0f)]
    float max_fuel = 200.0f;
    float fuel;
    public FuelGauge fuel_gauge;
    public FuelMeter fuel_meter;
    [SerializeField]
    [Range(0.0f, 3.0f)]
    float fuel_recharge_delay = 1.0f;
    [SerializeField]
    [Range(0.0f, 250.0f)]
    float fuel_recharge_rate = 100.0f;
    float elapsed_fuel_recharge_delay = 0.0f;

    // Audio
    AudioSource player_as;
    public AudioClip flying_sfx;
    public AudioClip game_over_sfx;

    // Progression
    public static int score = 0;
    public static int player_level = 0;
    public static int drones_destroyed = 0;
    int level_increase_rate = 10;

    // UI screens
    public static bool game_over = false;
    public static bool paused = false;
    public GameObject game_over_screen;
    public GameObject score_screen;
    public GameObject pause_screen;

    void Start()
    {
        InitialPlayerSetup();
    }
    void Update()
    {
        if (game_over)
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
    }
    private static void InitialProgressionSetup()
    {
        score = 0;
        player_level = 0;
        drones_destroyed = 0;
    }
    private static void InitialUISetup()
    {
        game_over = false;
        paused = false;
    }
    private void EnableGameOverScreen()
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
    private void EnablePauseScreen()
    {
        pause_screen.SetActive(true);
        score_screen.SetActive(false);
        if (player_as.isPlaying)
        {
            player_as.Stop();
        }
        Time.timeScale = 0.0f;
    }
    private void DisablePauseScreen()
    {
        pause_screen.SetActive(false);
        score_screen.SetActive(true);
        Time.timeScale = 1.0f;
    }
    private void DetermineCurrentPlayerLevel()
    {
        if (score / level_increase_rate <= 4)
        {
            player_level = score / level_increase_rate;
        }
        else
        {
            player_level = 4;
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
            player_as.PlayOneShot(flying_sfx, 1.0f);
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
        if (collision.gameObject.tag == "Drone Hit Box")
        {
            game_over = true;
        }
    }
}
