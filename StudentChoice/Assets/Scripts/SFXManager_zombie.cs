using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: TJ Carlson
 * Created: 4-27-2020
 * SFX Manager specifically for zombie sounds
 * Splitting separately from player and general SFX due to number of necessary concurrent sounds
 * IMPORTANT: number of attached audio channels must be a multiple of 3
 * 3 zombie SFX groups: attacking, walking, growling
 * growling: set on loop for each nearby zombie
 * walking: either set on loop or do 2 alternating
 * attacking: simple swipe mechanic
 */
public class SFXManager_zombie : MonoBehaviour
{
    static int s_len; // total number of sources
    static int current_channel;
    static AudioSource[] audio_channels;
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
