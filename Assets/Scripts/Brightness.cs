using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brightness : MonoBehaviour
{
    public GameObject lightObject;
    Light light;
    Slider slider;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        light = lightObject.GetComponent<Light>();
        slider.value = PlayerPrefs.GetFloat("lightFloat", 0f);
    }

    // Start is called before the first frame update
    void Update()
    {
        slider.onValueChanged.AddListener(delegate {changeBrightness();});
    }

    void changeBrightness()
    {
        PlayerPrefs.SetFloat("lightFloat", slider.value);
        light.intensity = slider.value;
    }
}
