using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandController : MonoBehaviour
{

    private ActionBasedController left_controller;
    private float trigger_pressed;
    private Player player_script;
    // Start is called before the first frame update
    void Start()
    {
        left_controller = GetComponent<ActionBasedController>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        trigger_pressed = left_controller.selectAction.action.ReadValue<float>();

        left_controller.selectAction.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        trigger_pressed = left_controller.selectAction.action.ReadValue<float>();
        if (trigger_pressed == 1.0f)
        {
            player_script.gun_trigger_pressed = true;
        }

        // need similar stuff but to know direction gun is pointing for laser
        //Vector3 controller_facing_direction = gameObject.transform.rotation * Vector3.forward;
        //player_script.UpdateDirectionAndThrottleValues(trigger_pressed, controller_facing_direction);
    }

    void Update()
    {

    }
}
