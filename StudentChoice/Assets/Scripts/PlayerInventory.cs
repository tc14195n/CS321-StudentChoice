using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public LoadoutManager loadout;

    // Start is called before the first frame update
    void Start()
    {
        loadout = GetComponentInChildren<LoadoutManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
