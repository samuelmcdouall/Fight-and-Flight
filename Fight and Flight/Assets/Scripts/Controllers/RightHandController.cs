using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RightHandController : MonoBehaviour
{

    private ActionBasedController right_controller;
    private float amount_pressed;
    // Start is called before the first frame update
    void Start()
    {
        right_controller = GetComponent<ActionBasedController>();

        amount_pressed = right_controller.selectAction.action.ReadValue<float>();

        right_controller.selectAction.action.performed += Action_performed;
    }

    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_pressed = right_controller.selectAction.action.ReadValue<float>();
        print("amount trigger pressed (from 0 to 1): " + amount_pressed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
