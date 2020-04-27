using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager_player : MonoBehaviour
{
    int s_len, current_channel; // total number of sources
    public AudioSource[] audio_channels;
    public AudioClip p_walk;
    public AudioClip gun_single;
    public AudioClip gun_burst;
    public AudioClip gun_auto;
    public AudioClip[] p_hurt;
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

    public void walk()
    {
        audio_channels[current_channel].clip = p_walk;
        playClip();
    }
    public void hurt()
    {
        audio_channels[current_channel].clip = p_hurt[0];
        playClip();
    }
    public void attack()
    {
        audio_channels[current_channel].clip = gun_single;
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
