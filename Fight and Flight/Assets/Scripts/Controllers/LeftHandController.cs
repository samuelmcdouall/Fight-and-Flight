using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandController : MonoBehaviour
{

    private ActionBasedController left_controller;
    private float trigger_pressed;
    private Gun gun_script;
    // Start is called before the first frame update
    void Start()
    {
        left_controller = GetComponent<ActionBasedController>();
        gun_script = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();

        trigger_pressed = left_controller.selectAction.action.ReadValue<float>();
        
        left_controller.selectAction.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Gun.ammo != 0)
        {
            trigger_pressed = left_controller.selectAction.action.ReadValue<float>();
            if (trigger_pressed == 1.0f)
            {
                gun_script.gun_trigger_pressed = true;
            }
        }
    }

    void Update()
    {

    }
}
