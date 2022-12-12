using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    //public GameObject listenerObject;
    //AudioListener listener; 

    Slider slider;

    void Start()
    {
        //listener = listenerObject.GetComponent<AudioListener>();
        slider = gameObject.GetComponent<Slider>();
    }

    // Start is called before the first frame update
    void Update()
    {
        slider.onValueChanged.AddListener(delegate {changeVolume();});
    }

    void changeVolume()
    {
        AudioListener.volume = slider.value;
    }
}
