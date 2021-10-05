using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{

    private ActionBasedController right_controller;
    private float amount_trigger_pressed;
    float press_threshold = 0.001f;
    private Player player_script;
    // Start is called before the first frame update
    void Start()
    {
        right_controller = GetComponent<ActionBasedController>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        amount_trigger_pressed = right_controller.selectAction.action.ReadValue<float>();

        right_controller.selectAction.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_trigger_pressed = right_controller.selectAction.action.ReadValue<float>();
        if (amount_trigger_pressed <= press_threshold)
        {
            player_script.throttle = false;
        }
        else
        {
            player_script.throttle = true;
        }
        Vector3 controller_facing_direction = gameObject.transform.rotation * Vector3.forward;
        player_script.UpdateDirectionAndThrottleValues(amount_trigger_pressed, controller_facing_direction);
    }

    void Update()
    {

    }
}
