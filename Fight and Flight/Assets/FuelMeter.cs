using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelMeter : MonoBehaviour
{
    // Start is called before the first frame update
    public Material fuel_meter_m;
    public Color purple;
    public Color orange;

    public void SetGlidingColour() {
        fuel_meter_m.SetColor("_Color", orange);
    }
    public void SetNonGlidingColour() {
        fuel_meter_m.SetColor("_Color", purple);
    }


}
