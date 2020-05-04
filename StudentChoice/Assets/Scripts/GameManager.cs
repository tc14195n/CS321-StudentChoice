using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score;
    int scene_num;
    string win_msg, lose_msg;
    public Text display_msg;
    // Start is called before the first frame update
    void Start()
    {
        win_msg = "Mission accomplished";
        lose_msg = "Mission failed";
        scene_num = SceneManager.GetActiveScene().buildIndex; 
        switch (scene_num)
        {
            case (0):
                GameData.hasWon = false;
                break;
            case (1):
                GameData.zombie_count = 2;
                break;
            case (2):
                GameData.zombie_count = 2;
                break;
            case (3):
                GameData.zombie_count = 2;
                break;
            case (4):
                if (GameData.hasWon)
                    display_msg.text = win_msg;
                else
                    display_msg.text = lose_msg;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("zombie_count: " + GameData.zombie_count);
        switch (scene_num)
        {
            case (0):
                if (Input.GetKeyDown(KeyCode.Space))
                    UnityEngine.SceneManagement.SceneManager.LoadScene(1);
                break;
            case (1):
                if (GameData.zombie_count <= 0)
                    SceneManager.LoadScene(scene_num + 1);
                break;
            case (2):
                if (GameData.zombie_count <= 0)
                    SceneManager.LoadScene(scene_num + 1);
                break;
            case (3):
                if (GameData.zombie_count <= 0) {
                    GameData.hasWon = true;
                    SceneManager.LoadScene(scene_num + 1);
                }
                break;
            case (4):
                if (Input.GetKeyDown(KeyCode.Space))
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                break;
        }
    }
}
