using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SingleSlider : MonoBehaviour
{
    #region Variables

    private Slider _slider;

    public bool IsEnabled
    {
        get { return _slider.interactable; }
        set { _slider.interactable = value; }

    }
    public float Value
    {
        get { return _slider.value; }
        set
        {
            _slider.value = value;
            _slider.onValueChanged.Invoke(_slider.value);
        }
    }
    public bool WholeNumbers
    {
        get { return _slider.wholeNumbers; }
        set { _slider.wholeNumbers = value; }
    }

    #endregion

    private void Awake()
    {
        if (!TryGetComponent<Slider>(out _slider))
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogError("Missing Slider Component");
#endif
        }
    }

    public void Setup(float value, float minValue, float maxValue, UnityAction<float> valueChanged)
    {
        _slider.minValue = minValue;
        _slider.maxValue = maxValue;

        _slider.value = value;
        _slider.onValueChanged.AddListener(Slider_OnValueChanged);
        _slider.onValueChanged.AddListener(valueChanged);
    }

    private void Slider_OnValueChanged(float arg0)
    {
    }

}