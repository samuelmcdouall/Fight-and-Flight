using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{

    private ActionBasedController right_controller;
    private float amount_trigger_pressed;
    private float menu_button_pressed;
    float press_threshold = 0.001f;
    private Player player_script;
    // Start is called before the first frame update
    void Start()
    {
        right_controller = GetComponent<ActionBasedController>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        right_controller.selectAction.action.performed += Trigger_Pressed;
        right_controller.activateAction.action.performed += Menu_Button_Pressed;
    }

    public void Menu_Button_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    { 
        player_script.PauseUnpause();
    }

    public void Trigger_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
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
        player_script.UpdateThrottleValue(amount_trigger_pressed);
    }

    public void RemoveActions()
    {
        right_controller.selectAction.action.performed -= Trigger_Pressed;
        right_controller.activateAction.action.performed -= Menu_Button_Pressed;
    }

    void Update()
    {
        Vector3 controller_facing_direction = gameObject.transform.rotation * Vector3.forward;
        player_script.UpdateDirection(controller_facing_direction);
    }
}
