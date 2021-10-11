using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
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
    public AudioClip gliding_sfx;
    public static int score = 0;
    public static int player_level = 0;
    int level_increase_rate = 10;
    float y_out_of_bounds = 0.5f;
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
    }

    public void UpdateDirectionAndThrottleValues(float trigger_amount, Vector3 direction)
    {
        current_throttle = trigger_amount;
        current_direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        //print("elapsed fuel recharge delay: " + elapsed_fuel_recharge_delay);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Drone Hit Box")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
