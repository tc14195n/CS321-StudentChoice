using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public Gun[] guns;
    public Gun currentWeapon;
    private int currentWeaponIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = guns[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
