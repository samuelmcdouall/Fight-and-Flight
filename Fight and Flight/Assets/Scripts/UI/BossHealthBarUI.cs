using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Slider gauge;
    public Gradient gradient;
    public Image fill;

    public void SetMaxBossBar(float max_hp)
    {
        gauge.maxValue = max_hp;
        gauge.value = max_hp;
        fill.color = gradient.Evaluate(1.0f);
    }

    public void SetBossBar(float current_hp)
    {
        gauge.value = current_hp;
        fill.color = gradient.Evaluate(gauge.normalizedValue);
    }

    public void SetBossBarInvulnerable(float current_hp)
    {
        gauge.value = current_hp;
        fill.color = Color.blue;
    }
}
