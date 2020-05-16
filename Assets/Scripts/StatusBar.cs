using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    Slider slider;
    public Image fill, mask;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void setTotalDroids(int total)
    {
        slider.maxValue = total;
    }

    public void changePlayerDroids(int player)
    {
        slider.value = player;
    }

    public void setStatusBarColurs(Color playerCol, Color enemyCol)
    {
        fill.color = playerCol;
        mask.color = enemyCol;

    }
}
