using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHealth : MonoBehaviour
{
    public float limbhealth = 25;
    

    public void DismemberLimb(GameObject limb) //delete the limb
    {

        //TODO: add gore or body part
        //GameObject bodyPart = Instantiate(limb, limb.transform.position, limb.transform.rotation);
        Destroy(limb);
        //gameObject.SetActive(false);
    }
}
