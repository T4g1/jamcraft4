using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
    private Slider slider;
    private float maximalValue = 1.0f;
    private float currentValue = 0.0f;


    void Awake()
    {
        slider = GetComponentInChildren<Slider>();

        Assert.IsNotNull(slider);
    }

    public void SetMaximalValue(float value)
    {
        maximalValue = value;
        UpdateSlider();
    }

    public void SetCurrentValue(float value)
    {
        currentValue = value;
        UpdateSlider();
    }

    void UpdateSlider()
    {
        slider.maxValue  = maximalValue;
        slider.value = currentValue;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
