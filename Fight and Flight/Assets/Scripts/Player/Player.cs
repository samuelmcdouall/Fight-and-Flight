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
    [Range(0.0f, 100.0f)]
    float max_fuel = 100.0f;
    float fuel;
    Vector3 starting_position;
    public FuelGauge fuel_gauge;
    public FuelMeter fuel_meter;
    [SerializeField]
    [Range(0.0f, 3.0f)]
    float fuel_recharge_delay = 1.0f;
    [SerializeField]
    [Range(0.0f, 50.0f)]
    float fuel_recharge_rate = 40.0f;
    float elapsed_fuel_recharge_delay = 0.0f;
    public static int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        starting_position = transform.position;
        ground_check = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<GroundCheck>();
        player_rb = GetComponent<Rigidbody>();
        fuel = max_fuel;
        fuel_gauge.SetMaxFuelGauge(max_fuel);
        fuel_meter.SetNonGlidingColour();
        score = 0;
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
        CheckIfOutOfBounds();
        CheckIfAbleToRechargeEnergy();
        if (throttle && fuel != 0.0f)
        {
            Fly();
        }
        else if (throttle && fuel == 0.0f)
        {
            Glide();
        }
        else
        {
            fuel_meter.SetNonGlidingColour();
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
        if (transform.position.y <= -20.0f)
        {
            transform.position = starting_position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
