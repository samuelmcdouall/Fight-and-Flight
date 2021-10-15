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
    void Start()
    {
        InitialSetupLeftController();
    }

    private void InitialSetupLeftController()
    {
        left_controller = GetComponent<ActionBasedController>();
        left_controller.selectAction.action.performed += Trigger_Pressed;
        left_controller.activateAction.action.performed += Menu_Button_Pressed;
        gun_script = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void Trigger_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_trigger_pressed = left_controller.selectAction.action.ReadValue<float>();
        bool trigger_fully_pressed = amount_trigger_pressed == 1.0f;
        if (player_script.in_menu && Player.game_over && trigger_fully_pressed)
        {
            print("QUIT GAME");
            //Application.Quit();
        }
        else
        {
            if ((Player.game_over || Player.victory) && trigger_fully_pressed)
            {
                LoadGameScene();
            }
            else if (Gun.ammo != 0 && !Player.paused && trigger_fully_pressed)
            {
                gun_script.gun_trigger_pressed = true;
            }
        }
    }

    private void LoadGameScene()
    {
        right_hand_controller.RemoveActions();
        RemoveActions();
        SceneManager.LoadScene("GameScene");
    }

    public void Menu_Button_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!player_script.in_menu)
        {
            player_script.PauseUnpause();
        }
    }
    public void RemoveActions()
    {
        left_controller.selectAction.action.performed -= Trigger_Pressed;
        left_controller.activateAction.action.performed -= Menu_Button_Pressed;
    }
}
