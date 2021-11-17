using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedDroneAnimation : MonoBehaviour
{
    Animator drone_ani;
    bool ready_to_trigger_damage_animation;
    void Start()
    {
        drone_ani = GetComponent<Animator>();
        ready_to_trigger_damage_animation = true;
    }
    void Update()
    {
        if (!ready_to_trigger_damage_animation)
        {
            if (drone_ani.GetCurrentAnimatorStateInfo(0).IsName("DroneTakeDamage"))
            {
                ready_to_trigger_damage_animation = true;
            }
        }
    }

    public void PlayDamageAnimation()
    {
        if (ready_to_trigger_damage_animation && drone_ani.GetCurrentAnimatorStateInfo(0).IsName("DroneIdle"))
        {
            drone_ani.SetTrigger("take_damage");
            ready_to_trigger_damage_animation = false;
        }
    }
}
