using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //TODO: make sure on game restart we don't try to access out-of-bounds value e.g. if(buildIndex > buildIndex.length)
    }
}
