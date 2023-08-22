using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource m_music;
    private AudioSource m_sound;
    public const string key_music = "key_music";
    public const string key_sound = "key_sound";

    private void Awake()
    {
        instance = this;
        
        m_music = transform.Find("Music").GetComponent<AudioSource>();
        m_sound = transform.Find("Sound").GetComponent<AudioSource>();
        
        UpdateVolume();
    }
    
    private void OnDestroy()
    {
        instance = null;
    }
    //播放背景音乐
    public void PlayMusic(AudioClip clip)
    {
        m_music.clip = clip;
        m_music.Play();
    }
    //播放音效
    public void PlaySound(AudioClip clip)
    {
        m_sound.PlayOneShot(clip);
    }
    
    //更新音量大小
    public void UpdateVolume()
    {
        m_music.volume = PlayerPrefs.GetFloat(key_music, 0.5f);
        m_sound.volume = PlayerPrefs.GetFloat(key_sound, 0.5f);
    }

    
}
