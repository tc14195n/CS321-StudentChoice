using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    
    public Image healthSlider;
    public Text ammoLabel;
    public Text ammoLabel2;

    private PlayerInventory inv;
    private PlayerHealthController pHC;
    //private Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        pHC = GetComponent<PlayerHealthController>();
        inv = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {

        healthSlider.fillAmount = pHC.health / 100;
        if (pHC.health <= 30)
        {
            healthSlider.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
        }
        ammoLabel.text = inv.loadout.currentWeapon.ammoLeftInMag.ToString() + " /";
    }
}
