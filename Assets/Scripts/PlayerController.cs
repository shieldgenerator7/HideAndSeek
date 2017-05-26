using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float speed = 3.0f;
    public float rotSpeed = 7.0f;
    public float jumpForce = 10.0f;
    public GameObject cameraPrefab;
    private Camera viewCamera;

    private Rigidbody rb;
    private float distToGround;
    private float jumpTime = 0;
    private float jumpDuration = 1.0f;//how long a jump can have effect

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameObject cam = Instantiate(cameraPrefab);
            cam.transform.parent = transform;
            viewCamera = cam.GetComponent<Camera>();
            viewCamera.tag = "MainCamera";
            rb = GetComponent<Rigidbody>();
            distToGround = GetComponent<BoxCollider>().bounds.extents.y;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isLocalPlayer)
        {
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;
            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            transform.Translate(moveX, 0, moveZ);
            float rotX = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
            float rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
            transform.Rotate(0, rotX, 0);//yes, apparently it's correct to switch the X and Y here
            viewCamera.transform.Rotate(-rotY, 0, 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded()) {
                    Debug.Log("Grounded!");
                    rb.AddForce(0, jumpForce*rb.mass, 0);
                }
                else
                {
                    Debug.Log("In Air! jumpTime: "+jumpTime+"; Time: "+Time.time);
                }
            }
        }
    }
    //2017-05-25: copied from an answer by aldonaletto: http://answers.unity3d.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
    bool isGrounded()
    {
        float buffer = 0.2f;
        return Physics.Raycast(transform.position+(Vector3.down*(distToGround)), Vector3.down, buffer);
    }
}
