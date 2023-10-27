using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    //Attach this file to a Unity Slider and set the min to 0.0001
    //Be sure to make an audio mixer and expose the main volume
    public AudioMixer mixer;

    public string mixerName;
    [SerializeField] private Slider slider;

    
    //sets initial level of volume
    private void Start()
    {
        float value = 0f;
        mixer.GetFloat(mixerName, out value);

        slider.value = Mathf.Pow(10, value / 20);
    }
    
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat(mixerName, Mathf.Log10(sliderValue) * 20);
    }
}
