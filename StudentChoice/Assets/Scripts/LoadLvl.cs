using UnityEngine;

public class LoadLvl : MonoBehaviour
{
    public int lvl = 1;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UnityEngine.SceneManagement.SceneManager.LoadScene(lvl);
    }
}
