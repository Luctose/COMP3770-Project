using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioSource ambience;
    public AudioSource fire;
    public AudioSource kPerry;

    float ambVol;
    float fireVol;
    float perryVol;

    Slider slider;

    void Start()
    {
        slider = gameObject.GetComponent<Slider>();
        ambVol = ambience.volume;
        fireVol = fire.volume;
        perryVol = kPerry.volume;
    }

    // Start is called before the first frame update
    void Update()
    {
        slider.onValueChanged.AddListener(delegate {changeVolume();});
    }

    void changeVolume()
    {
        ambience.volume = ambVol * slider.value;
        fire.volume = fireVol * slider.value;
        kPerry.volume = perryVol * slider.value;
    }
}
