using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    
    public Image healthSlider, healthSlider2; // using 2 healthbars as I wanted the health to look like it is draining inwards instead of just to the left or rigth
    public Color color;
    public Image crosshair;
    public Text ammoMag;
    public Text ammoReserve;
    public Text firemode;

    private PlayerInventory inv;
    private PlayerHealthController pHC;


    // Start is called before the first frame update
    void Start()
    {
        pHC = GetComponent<PlayerHealthController>();
        inv = GetComponent<PlayerInventory>();

        healthSlider.color = color;
        healthSlider2.color = color;
    }

    // Update is called once per frame
    void Update()
    {

        healthSlider.fillAmount = (pHC.health / 100) / 2;
        healthSlider2.fillAmount = (pHC.health / 100) / 2;
        if (pHC.health > 30)
        {
            healthSlider.color = color; // this is to make sure that if we regen health we return to the original color
            healthSlider2.color = color;
        }
        if (pHC.health <= 30)
        {
            healthSlider.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1)); //fades through white and red to make the health bar show that your health is critical
            healthSlider2.color = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, 1)); // This happens for both healthbars
        }

        ammoMag.text = inv.loadout.currentWeapon.getAmmoCount().ToString() + " /"; //updates to screen the ammo count in the mag
        //TODO: update ammo count in reserve
        crosshair.enabled = !inv.loadout.currentWeapon.ADSing(); //disables the crosshair when you ads
        firemode.text = inv.loadout.currentWeapon.getFiremode(); //updates to screen what mode the weapon is on
    }
}
