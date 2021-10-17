using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    Text ammo_remaining_text;
    void Start()
    {
        ammo_remaining_text = GetComponent<Text>();
        ammo_remaining_text.text = "" + Gun.ammo;
    }
    void Update()
    {
        if (Player.in_menu)
        {
            ammo_remaining_text.text = "";
        }
        else
        {
            DetermineGunColour();
            ammo_remaining_text.text = "" + Gun.ammo;
        }
    }

    private void DetermineGunColour()
    {
        if (Gun.ammo <= 2)
        {
            ammo_remaining_text.color = Color.red;
        }
        else if (Gun.ammo <= 6)
        {
            ammo_remaining_text.color = Color.yellow;
        }
        else if (Gun.ammo <= 10)
        {
            ammo_remaining_text.color = Color.green;
        }
    }
}
