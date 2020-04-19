using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbHealth : MonoBehaviour
{
    public float limbhealth = 25;
    

    public void DismemberLimb(GameObject limb)
    {
        //GameObject bodyPart = Instantiate(limb, limb.transform.position, limb.transform.rotation);
        //TODO: add gore or body part
        Destroy(limb);
        //gameObject.SetActive(false);
    }
}
