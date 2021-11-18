using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    Text ammo_remaining_text;
    void Start()
    {
        ammo_remaining_text = GetComponent<Text>();
        ammo_remaining_text.text = "" + GunPCTest.ammo;
    }
    void Update()
    {
        if (PlayerPCTest.in_menu)
        {
            ammo_remaining_text.text = "";
        }
        else
        {
            DetermineGunColour();
            ammo_remaining_text.text = "" + GunPCTest.ammo;
        }
    }

    void DetermineGunColour()
    {
        if (GunPCTest.ammo <= 2)
        {
            ammo_remaining_text.color = Color.red;
        }
        else if (GunPCTest.ammo <= 6)
        {
            ammo_remaining_text.color = Color.yellow;
        }
        else if (GunPCTest.ammo <= 10)
        {
            ammo_remaining_text.color = Color.green;
        }
    }
}
