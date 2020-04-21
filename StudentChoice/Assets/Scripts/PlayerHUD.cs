using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    
    public Image healthSlider, healthSlider2;
    public Image crosshair;
    public Text ammoMag;
    public Text ammoReserve;
    public Text firemode;

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

        healthSlider.fillAmount = (pHC.health / 100) / 2;
        healthSlider2.fillAmount = (pHC.health / 100) / 2;
        if (pHC.health <= 30)
        {
            healthSlider.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
            healthSlider2.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1));
        }
        ammoMag.text = inv.loadout.currentWeapon.getAmmoCount().ToString() + " /";
        crosshair.enabled = !inv.loadout.currentWeapon.ADSing();
        firemode.text = inv.loadout.currentWeapon.getFiremode();
    }
}
