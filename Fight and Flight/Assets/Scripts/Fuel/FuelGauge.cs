using UnityEngine;
using UnityEngine.UI;

public class FuelGauge : MonoBehaviour
{
    public Slider gauge;
    public Gradient gradient;
    public Image fill;

    public void SetMaxFuelGauge(float max_fuel)
    {
        gauge.maxValue = max_fuel;
        gauge.value = max_fuel;
        fill.color = gradient.Evaluate(1.0f);
    }

    public void SetFuelGauge(float current_fuel)
    {
        gauge.value = current_fuel;
        fill.color = gradient.Evaluate(gauge.normalizedValue);
    }
}
