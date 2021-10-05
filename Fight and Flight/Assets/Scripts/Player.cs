using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        starting_position = transform.position;
        ground_check = GameObject.FindGameObjectWithTag("GroundCheck").GetComponent<GroundCheck>();
        player_rb = GetComponent<Rigidbody>();
        fuel = max_fuel;
    }

    public void UpdateDirectionAndThrottleValues(float trigger_amount, Vector3 direction)
    {
        current_throttle = trigger_amount;
        current_direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
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
            //player_rb.velocity = player_rb.velocity;
        }
    }

    private void Glide()
    {
        player_rb.velocity = new Vector3(current_direction.x * glide_speed, -1.0f, current_direction.z * glide_speed);
    }

    private void Fly()
    {
        player_rb.velocity = current_direction * current_throttle * fly_speed;
        fuel -= current_throttle;
        if (fuel < 0.0f)
        {
            fuel = 0.0f;
        }
    }

    private void CheckIfAbleToRechargeEnergy()
    {
        if (ground_check.is_grounded)
        {
            fuel = max_fuel;
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
            transform.position = starting_position;
        }
    }
}
