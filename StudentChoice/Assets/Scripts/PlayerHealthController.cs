﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealthController : MonoBehaviour
{
    public float health = 100;
    public AudioClip hurtSFX;
    private AudioSource aS;

    void Start(){
        aS = GetComponent<AudioSource>();
    }
    //void Update(){}

    public void damage(float amount)
    {
        //TODO: camera shake or bloody screen and hit/moan sound
        //TODO: HUD update
        health -= amount;
        if(!aS.isPlaying)
        aS.PlayOneShot(hurtSFX);


        if (health <= 0) //death check
        {
            die();
        }
    }

    public void die()
    {
        //can spawn a ragdoll of player and a camera to see the player's body fall and maybe have zombies bite the body
        GameData.hasWon = false;
        SceneManager.LoadScene(4);
    }
}
