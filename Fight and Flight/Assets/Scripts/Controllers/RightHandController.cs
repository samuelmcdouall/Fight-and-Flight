using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{

    private ActionBasedController right_controller;
    private float amount_pressed;
    private Player player_script;
    // Start is called before the first frame update
    void Start()
    {
        right_controller = GetComponent<ActionBasedController>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        amount_pressed = right_controller.selectAction.action.ReadValue<float>();

        right_controller.selectAction.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_pressed = right_controller.selectAction.action.ReadValue<float>();
        if (amount_pressed <= 0.001f)
        {
            player_script.throttle = false;
        }
        else
        {
            player_script.throttle = true;
        }
        //print("amount trigger pressed (from 0 to 1): " + amount_pressed);
        player_script.Fly(amount_pressed, gameObject.transform.rotation * Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
