using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float speed = 3.0f;
    public float rotSpeed = 7.0f;
    public float jumpForce = 20.0f;
    public float jumpDuration = 0.5f;//how long a jump can have effect
    public GameObject cameraPrefab;
    private Camera viewCamera;

    private Rigidbody rb;
    private CapsuleCollider bc;
    private float distToGround;
    private float jumpTime = 0;
    
	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameObject cam = Instantiate(cameraPrefab);
            cam.transform.parent = transform;
            cam.transform.localPosition = Vector3.zero;
            viewCamera = cam.GetComponent<Camera>();
            viewCamera.tag = "MainCamera";
            rb = GetComponent<Rigidbody>();
            bc = GetComponent<CapsuleCollider>();
            distToGround = bc.bounds.extents.y;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isLocalPlayer)
        {
            float moveZ = Input.GetAxis("Vertical") * speed * rb.mass;
            float moveX = Input.GetAxis("Horizontal") * speed * rb.mass;
            rb.AddRelativeForce(moveX, 0, moveZ);
            float rotX = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
            float rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
            transform.Rotate(0, rotX, 0);//yes, apparently it's correct to switch the X and Y here
            viewCamera.transform.Rotate(-rotY, 0, 0);
            if (Input.GetKey(KeyCode.Space))
            {
                if (isGrounded())
                {
                    jumpTime = Time.time + jumpDuration;
                }
                if (jumpTime > Time.time)
                {
                    rb.AddForce(0, jumpForce*rb.mass, 0);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    //2017-05-25: copied from an answer by aldonaletto: http://answers.unity3d.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html
    bool isGrounded()
    {
        float buffer = 0.2f;
        Vector3 extents = bc.bounds.extents;
        return Physics.CheckBox(transform.position + (Vector3.down * (distToGround + buffer)), new Vector3(extents.x, buffer / 2, extents.z));
    }
}
