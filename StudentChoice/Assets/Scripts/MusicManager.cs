using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager BGM; //maybe add "battle" music
    //public static bool playing;
    public AudioClip song;
    private AudioSource aS;
    public float volume;

    // Start is called before the first frame update

    void Start() // play music but only checks when you are in the main menu
    {
        aS = GetComponent<AudioSource>();

        if (!aS.isPlaying && Counter.count != 1)
        {
            aS.PlayOneShot(song, volume);
            DontDestroyOnLoad(this.gameObject);
            Counter.count = 1;
            GameManager.hasWon = false;
           // playing = true;
        }
    }

    private void Update() // play again if the music stops doesnt loop on main menu
    {
        if (!aS.isPlaying && SceneManager.GetActiveScene().buildIndex > 0)
        {
            aS.PlayOneShot(song, volume);
            DontDestroyOnLoad(this.gameObject);
           // playing = true;
        }
    }
 
}
