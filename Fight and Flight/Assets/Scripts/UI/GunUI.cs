using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    Text ammo_remaining_text;
    void Start()
    {
        ammo_remaining_text = GetComponent<Text>();
        ammo_remaining_text.text = "" + Player.ammo;
    }
    void Update()
    {
        if (Player.ammo <= 1)
        {
            ammo_remaining_text.color = Color.red;
        }
        else if (Player.ammo <= 3)
        {
            ammo_remaining_text.color = Color.yellow;
        }
        else if (Player.ammo <= 5)
        {
            ammo_remaining_text.color = Color.green;
        }
        ammo_remaining_text.text = "" + Player.ammo;
    }
}
