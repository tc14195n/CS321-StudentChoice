using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wirefenceController : MonoBehaviour
{
    public PlayerHealthController phc;
    BoxCollider col_fence;
    int damage;
    float damage_timer;
    // Start is called before the first frame update
    void Start()
    {
        damage_timer = 0;
        damage = 2;
        col_fence = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        damage_timer += Time.deltaTime;
        if (damage_timer % 1 == 0)
            phc.damage(damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            phc.damage(damage);
    }
    private void OnTriggerExit(Collider other)
    {
        damage_timer = 0;
    }
}
