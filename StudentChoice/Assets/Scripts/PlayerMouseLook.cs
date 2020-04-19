using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    public float sens;
    private float mouseX, mouseY;
    public Transform player;
    private float rotationX = 0f;


    // Start is called before the first frame update
    void Start()
    {
       // player = GetComponentInParent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);
        transform.localRotation = Quaternion.Euler(rotationX, player.rotation.y, player.rotation.y);
        player.Rotate(Vector3.up * mouseX);
    }
}
