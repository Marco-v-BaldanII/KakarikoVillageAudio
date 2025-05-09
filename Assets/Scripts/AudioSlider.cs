using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    private Slider slider;
    public AudioMixer mixer;
    public string parameter;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        float decibels = 20 * Mathf.Log10(slider.value);
        if (slider.value == 0) { decibels = -80; }
        mixer.SetFloat( parameter, decibels );
    }
}
