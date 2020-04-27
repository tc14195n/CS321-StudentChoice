using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager BGM; //maybe add "battle" music
    //public static bool playing;
    public AudioClip song;
    private AudioSource source;
    public float volume;
    public List<AudioClip> MainMenu, Level1, Level2, Level3, EndMenu;

    // Start is called before the first frame update

    void Start() // play music but only checks when you are in the main menu
    {
        source = GetComponent<AudioSource>();

        if (!source.isPlaying && GameData.playthrough_count != 1)
        {
            source.PlayOneShot(song, volume);
            DontDestroyOnLoad(this.gameObject);
            GameData.playthrough_count = 1;
            GameManager.hasWon = false;
           // playing = true;
        }
    }

    private void Update() // play again if the music stops doesnt loop on main menu
    {
        if (!source.isPlaying && SceneManager.GetActiveScene().buildIndex > 0)
        {
            source.PlayOneShot(song, volume);
            DontDestroyOnLoad(this.gameObject);
           // playing = true;
        }
    }
 
}
