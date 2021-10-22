using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;

public class RightHandController : MonoBehaviour
{
    private ActionBasedController right_controller;
    private float amount_trigger_pressed;
    float press_threshold;
    private Player player_script;
    public LeftHandController left_hand_controller;
    void Start()
    {
        InitialSetupRightController();
    }
    void Update()
    {
        Vector3 controller_facing_direction = gameObject.transform.rotation * Vector3.forward;
        player_script.UpdateDirection(controller_facing_direction);
    }

    private void InitialSetupRightController()
    {
        right_controller = GetComponent<ActionBasedController>();
        right_controller.selectAction.action.performed += Trigger_Pressed;
        right_controller.activateAction.action.performed += Menu_Button_Pressed;
        player_script = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        press_threshold = 0.001f;
    }

    public void Trigger_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        amount_trigger_pressed = right_controller.selectAction.action.ReadValue<float>();
        bool trigger_fully_pressed = amount_trigger_pressed == 1.0f;
        if (Player.in_menu && Player.game_over && trigger_fully_pressed)
        {
            LoadMenuScene();
        }
        else
        {
            if ((Player.game_over || Player.victory) && trigger_fully_pressed)
            {
                LoadMenuScene();
            }
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
    }

    private void LoadMenuScene()
    {
        left_hand_controller.RemoveActions();
        RemoveActions();
        SceneManager.LoadScene("MenuScene");
    }

    public void Menu_Button_Pressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        print("in menu: " + Player.in_menu);
        if (!Player.in_menu)
        {
            player_script.PauseUnpause();
        }
    }

    public void RemoveActions()
    {
        right_controller.selectAction.action.performed -= Trigger_Pressed;
        right_controller.activateAction.action.performed -= Menu_Button_Pressed;
    }
}
