using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager_player : MonoBehaviour
{
    public static SFXManager_player Instance;
    static int s_len; // total number of sources
    static int current_channel;
    static AudioSource[] audio_channels;
    //public AudioClip p_walk;
    //public AudioClip gun_single;
    //public AudioClip gun_burst;
    //public AudioClip gun_auto;
    //public AudioClip[] p_hurt;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        audio_channels = GetComponents<AudioSource>();
        s_len = audio_channels.Length;
        current_channel = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public static void playClip(AudioClip clip)
    {
        audio_channels[current_channel].clip = clip;
        audio_channels[current_channel].volume = 1;
        audio_channels[current_channel].Play();
        current_channel++;
        if (current_channel == s_len)
            current_channel = 0;
    }
}
