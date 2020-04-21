using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float x, z;
    private CharacterController cC;
    public float moveSpeed;
    public bool isMoving;

    
    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");  
        
        if(x != 0 || z !=0 )
        {
            isMoving = true; //used for gun animations
        }

        cC.Move((transform.right * x + transform.forward *z) * moveSpeed * Time.deltaTime);
    }
}
