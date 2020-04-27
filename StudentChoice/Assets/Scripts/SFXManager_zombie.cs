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
    int s_len, current_channel; // total number of sources
    public AudioSource[] audio_channels;
    public AudioClip z_growl;
    public AudioClip z_attack;
    public AudioClip z_walk;
    // Start is called before the first frame update
    void Start()
    {
        audio_channels = GetComponents<AudioSource>();
        s_len = audio_channels.Length;
        current_channel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void growl()
    {
        audio_channels[current_channel].clip = z_growl;
        //volume adjust
        playClip();
    }
    public void walk()
    {
        audio_channels[current_channel].clip = z_walk;
        //volume adjust
        playClip();
    }
    public void attack()
    {
        audio_channels[current_channel].clip = z_attack;
        //volume adjust
        playClip();
    }
    private void playClip()
    {
        audio_channels[current_channel].Play();
        current_channel++;
        if (current_channel == s_len)
            current_channel = 0;
    }

}
