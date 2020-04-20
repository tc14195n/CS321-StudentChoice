using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRagdoll : MonoBehaviour
{
    public GameObject[] limbs;
    public GameObject[] limbJoints;

    /* Element 0: head
     * Element 1: ArmL
     * Element 2: ForeArmL
     * Element 3: HandL
     * Element 4: ArmR
     * Element 5: ForeArmR
     * Element 6: HandR
     * Element 7: LegL
     * Element 8: KneeL
     * Element 9: LegR
     * Element 10: KneeR
     */

    public void Dismember(bool[] bodyparts) //remove the limbs that are missing on the zombie (used for the ragdoll)
    {
        for(int i = 0; i < bodyparts.Length; i++)
        {
            if (bodyparts[i] == true && limbs[i].activeSelf != false) // if this limb is missing then...
            {                
                limbs[i].SetActive(false); //remove the missing limb
                limbJoints[i].SetActive(false); //remove limb colliders
            }
            
        }
    }
}
