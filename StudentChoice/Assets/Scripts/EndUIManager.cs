using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndUIManager : MonoBehaviour
{
    public Text condition;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.hasWon)
            condition.text = "You Survived";
        else
        {
            condition.text = "You Died";
            // maybe change font depending on win/lose
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
