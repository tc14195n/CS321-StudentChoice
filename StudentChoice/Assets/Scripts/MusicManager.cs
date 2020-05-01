using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager BGM; //maybe add "battle" music
    //public static bool playing;
    //public AudioClip song;
    private AudioSource source;
    public float volume;
    public List<AudioClip> MainMenu, Level1, Level2, Level3, EndMenu;
    int b_index, b_last;

    // Start is called before the first frame update

    void Start() // play music but only checks when you are in the main menu
    {
        b_last = 0;
        source = GetComponent<AudioSource>();
        source.loop = true;
        DontDestroyOnLoad(this);

        /*if (!source.isPlaying && GameData.playthrough_count != 1)
        {
            source.PlayOneShot(song, volume);
            DontDestroyOnLoad(this.gameObject);
            GameData.playthrough_count = 1;
            GameManager.hasWon = false;
           // playing = true;
        }*/
    }

    private void Update() // play again if the music stops doesnt loop on main menu
    {
        b_index = SceneManager.GetActiveScene().buildIndex;
        //if (b_index != 0)
        
        
        if(b_last != b_index)
        {
            source.volume = 0;
            switch (b_index)
            {
                case (0):
                    source.clip = MainMenu[0];
                    break;
                case (1):
                    source.clip = Level1[0];
                    break;
                case (2):
                    source.clip = Level2[0];
                    break;
                case (3):
                    source.clip = Level3[0];
                    break;
                case (4):
                    source.clip = EndMenu[0];
                    break;

            }
            source.Play();
        }
        
        if (source.volume != 1)
            source.volume += 0.1f * Time.deltaTime;
        /*if (!source.isPlaying && SceneManager.GetActiveScene().buildIndex > 0)
        {
            source.PlayOneShot(song, volume);
            DontDestroyOnLoad(this.gameObject);
           // playing = true;
        }*/
        b_last = b_index;
    }
 
}
