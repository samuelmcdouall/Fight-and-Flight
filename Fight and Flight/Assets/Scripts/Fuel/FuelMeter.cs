using UnityEngine;

public class FuelMeter : MonoBehaviour
{
    public Material fuel_meter_m;
    public Color non_gliding_colour;
    public Color gliding_colour;
    public void SetGlidingColour() {
        fuel_meter_m.SetColor("_Color", gliding_colour);
    }
    public void SetNonGlidingColour() {
        fuel_meter_m.SetColor("_Color", non_gliding_colour);
    }
}
