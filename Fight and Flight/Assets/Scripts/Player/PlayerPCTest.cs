using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPCTest : MonoBehaviour
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
    float throttle_rate;
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



    [SerializeField]
    float rotation_speed = 10.0f;
    float mouse_h;
    float mouse_v;
    [SerializeField]
    float mouse_v_min_clamp = -35.0f;
    [SerializeField]
    float mouse_v_max_clamp = 60.0f;

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
                HighScoreTracker.CheckAgainstCurrentHighScore();
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
            if (Input.GetMouseButtonDown(1))
            {
                LoadMenuScene();
            }
        }
        else if (game_over)
        {
            if (game_over_screen.activeSelf == false)
            {
                EnableGameOverScreen();
            }
            if (Input.GetMouseButtonDown(1))
            {
                LoadMenuScene();
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!in_menu) 
                {
                    LoadGameScene();
                }
                else
                {
                    print("Quit Application");
                    Application.Quit();
                }
            }
        }
        else if (paused)
        {
            if (pause_screen.activeSelf == false)
            {
                EnablePauseScreen();
            }
            if (Input.GetKeyDown(KeyCode.Space) && !in_menu)
            {
                statistics.SetActive(!statistics.activeSelf);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpause();
            }
        }
        else
        {
            if (pause_screen && pause_screen.activeSelf == true)
            {
                DisablePauseScreen();
            }
            DetermineCurrentPlayerLevel();
            CheckIfOutOfBounds();
            CheckIfAbleToRechargeEnergy();

            mouse_h += Input.GetAxis("Mouse X") * rotation_speed;
            mouse_v -= Input.GetAxis("Mouse Y") * rotation_speed;
            mouse_v = Mathf.Clamp(mouse_v, mouse_v_min_clamp, mouse_v_max_clamp);
            transform.rotation = Quaternion.Euler(mouse_v, mouse_h, 0.0f);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseUnpause();
            }
            if (Input.GetKeyDown(KeyCode.Space) && !in_menu)
            {
                statistics.SetActive(!statistics.activeSelf);
            }
        }
    }

    void FixedUpdate()
    {
        DetermineFlightSpeedAndDirection();
    }
    void InitialPlayerSetup()
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

    void GetPlayerComponents()
    {
        player_as = GetComponent<AudioSource>();
        player_rb = GetComponent<Rigidbody>();
        ground_check = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<GroundCheck>();
    }
    void InitialFuelSetup()
    {
        fuel = max_fuel;
        fuel_gauge.SetMaxFuelGauge(max_fuel);
        fuel_meter.SetNonGlidingColour();
        elapsed_fuel_recharge_delay = 0.0f;
        throttle = false;
        throttle_rate = 0.5f;
    }
    void InitialProgressionSetup()
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
    void InitialUISetup()
    {
        game_over = false;
        paused = false;
        victory = false;
        Cursor.visible = false;
    }
    void EnableGameOverScreen()
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
    void EnablePauseScreen()
    {
        pause_screen.SetActive(true);
        if (player_as.isPlaying)
        {
            player_as.Stop();
        }
        Time.timeScale = 0.0f;
    }
    void DisablePauseScreen()
    {
        pause_screen.SetActive(false);
        Time.timeScale = 1.0f;
    }
    void DetermineCurrentPlayerLevel()
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
    void CheckIfOutOfBounds()
    {
        if (transform.position.y <= y_out_of_bounds)
        {
            game_over = true;
            HighScoreTracker.CheckAgainstCurrentHighScore();
        }
    }
    void CheckIfAbleToRechargeEnergy()
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
    void DetermineFlightSpeedAndDirection()
    {
        current_direction = Camera.main.transform.forward;
        if (Input.GetMouseButton(1))
        {
            throttle = true;
        }
        else
        {
            throttle = false;
        }
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
    void Fly()
    {
        player_rb.velocity = current_direction * throttle_rate * fly_speed;
        fuel -= throttle_rate;
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
    void Glide()
    {
        player_rb.velocity = new Vector3(current_direction.x * glide_speed, -1.0f, current_direction.z * glide_speed);
        fuel_meter.SetGlidingColour();
    }

    public void PauseUnpause()
    {
        paused = !paused;
    }
    void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Drone Hit Box" || collision.gameObject.tag == "Boss Drone Hit Box" || collision.gameObject.tag == "Advanced Drone Hit Box")
        {
            game_over = true;
            HighScoreTracker.CheckAgainstCurrentHighScore();
        }
    }
}
