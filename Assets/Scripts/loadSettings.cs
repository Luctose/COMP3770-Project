using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadSettings : MonoBehaviour
{
    public GameObject levelLight;
    Light light;

    // Start is called before the first frame update
    void Start()
    {
        light = levelLight.GetComponent<Light>();
        AudioListener.volume = PlayerPrefs.GetFloat("volumeFloat", 1f);
        light.intensity = PlayerPrefs.GetFloat("lightFloat", 0f);
    }
}
