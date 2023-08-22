using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionView : ViewBase
{
    public Slider music;
    public Slider sound;

    private void Awake()
    {
        music.value = PlayerPrefs.GetFloat(AudioManager.key_music, 0.5f);
        sound.value = PlayerPrefs.GetFloat(AudioManager.key_sound, 0.5f);
    }

    public void OnSliderMusicChange(float value)
    {
        PlayerPrefs.SetFloat(AudioManager.key_music, value);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateVolume();
        }
    }
    
    public void OnSliderSoundChange(float value)
    {
        PlayerPrefs.SetFloat(AudioManager.key_sound, value);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.UpdateVolume();
        }
    }
    
    
}
