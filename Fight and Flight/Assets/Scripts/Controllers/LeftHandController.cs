using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class LeftHandController : MonoBehaviour
{
    ActionBasedController left_controller;
    float amount_trigger_pressed;
    Gun gun_script;
    Player player_script;
    bool game_over_haptic_activated;
    void Start()
    {
        InitialSetupLeftController();
    }

    void Update()
    {
        if (PlayerPCTest.game_over && !game_over_haptic_activated)
        {
            game_over_haptic_activated = true;
            left_controller.SendHapticImpulse(0.7f, 0.5f);
        }
    }

    void InitialSetupLeftController()
    {
        left_controller = GetComponent<ActionBasedController>();
        left_controller.selectAction.action.performed += Trigger_Pressed;
        left_controller.activateAction.action.performed += Menu_Button_Pressed;
        gun_script = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        game_over_haptic_activated = false;
    }
    void Trigger_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_trigger_pressed = left_controller.selectAction.action.ReadValue<float>();
        bool trigger_fully_pressed = amount_trigger_pressed == 1.0f;
        if (PlayerPCTest.in_menu && PlayerPCTest.game_over && trigger_fully_pressed)
        {
            Application.Quit();
        }
        else
        {
            if ((PlayerPCTest.game_over || PlayerPCTest.victory) && trigger_fully_pressed)
            {
                LoadGameScene();
            }
            else if (Gun.ammo != 0 && !PlayerPCTest.paused && trigger_fully_pressed)
            {
                gun_script.gun_trigger_pressed = true;
                left_controller.SendHapticImpulse(0.7f, 0.1f);
            }
        }
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Menu_Button_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!PlayerPCTest.in_menu)
        {
            player_script.PauseUnpause();
        }
    }
    void OnDestroy()
    {
        left_controller.selectAction.action.performed -= Trigger_Pressed;
        left_controller.activateAction.action.performed -= Menu_Button_Pressed;
    }
}
