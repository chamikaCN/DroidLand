using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Slider slider;
    public Gradient gradient;
    int maxHealth;
    public Image fill;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setMaxHealth(int health)
    {
        maxHealth = health;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void changeHealth(int newHealth)
    {
        slider.value = newHealth;
        fill.color = gradient.Evaluate(slider.value/5);
    }
}
