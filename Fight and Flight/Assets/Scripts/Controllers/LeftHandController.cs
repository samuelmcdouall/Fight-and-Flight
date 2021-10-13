using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class LeftHandController : MonoBehaviour
{

    private ActionBasedController left_controller;
    private float amount_trigger_pressed;
    private Gun gun_script;
    private Player player_script;
    public RightHandController right_hand_controller;
    // Start is called before the first frame update
    void Start()
    {
        left_controller = GetComponent<ActionBasedController>();
        gun_script = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        left_controller.selectAction.action.performed += Trigger_Pressed;
        left_controller.activateAction.action.performed += Menu_Button_Pressed;
    }

    public void Menu_Button_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!player_script.in_menu)
        {
            player_script.PauseUnpause();
        }
    }

    private void Trigger_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_trigger_pressed = left_controller.selectAction.action.ReadValue<float>();
        if (player_script.in_menu && Player.game_over && amount_trigger_pressed == 1.0f)
        {
            print("QUIT GAME");
            //Application.Quit();
        }
        else
        {
            if (Player.game_over && amount_trigger_pressed == 1.0f)
            {
                right_hand_controller.RemoveActions();
                RemoveActions();
                SceneManager.LoadScene("GameScene");
            }
            else if (Gun.ammo != 0 && !Player.paused && amount_trigger_pressed == 1.0f)
            {
                gun_script.gun_trigger_pressed = true;
            }
        }
    }
    public void RemoveActions()
    {
        left_controller.selectAction.action.performed -= Trigger_Pressed;
        left_controller.activateAction.action.performed -= Menu_Button_Pressed;
    }
}
