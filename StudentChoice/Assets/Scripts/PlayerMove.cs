using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    float x, z;
    private CharacterController cC;
    public float moveSpeed;
    public bool isMoving, isStunned;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector2 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        cC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isStunned)
        {
            //TODO: SEND THE HUD A STUN ALERT
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        velocity.y += -9.8f * Time.deltaTime;

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");  
        
        if((x != 0 || z !=0) && !isStunned)
        {
            isMoving = true; //used for gun animations
            cC.Move((transform.right * x + transform.forward * z) * moveSpeed * Time.deltaTime);
        }

       
        cC.Move(velocity * Time.deltaTime);
    }

}
