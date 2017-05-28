using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public static GameObject seeker;//true if this player is seeking the other players
    public bool isSeeker = false;

    public float speed = 3.0f;
    public float rotSpeed = 7.0f;
    public float jumpForce = 20.0f;
    public float jumpDuration = 0.5f;//how long a jump can have effect
    public GameObject stunVisionPrefab;//the prefab for the stun vision collider object
    public GameObject cameraPrefab;
    private Camera viewCamera;
    
    private CapsuleCollider bc;
    private CharacterController charCtr;
    private float distToGround;
    private float jumpTime = 0;
    private static Vector3 gravityVector = (Vector3.down * 9.81f);
    [SyncVar]
    private bool frozen = false;//can't move when true
    
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
            charCtr = GetComponent<CharacterController>();
            bc = GetComponent<CapsuleCollider>();
            distToGround = bc.bounds.extents.y;
            if (seeker == null)
            {
                seeker = gameObject;//set the seeker only for the first player
                isSeeker = true;
                GameObject svo = Instantiate(stunVisionPrefab);
                svo.transform.parent = cam.transform;
                svo.transform.localPosition = Vector3.zero;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isLocalPlayer && !frozen)
        {
            //Movement
            float moveZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            float moveX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            Vector3 moveDirection = transform.TransformDirection(new Vector3(moveX, 0, moveZ));
            
            if (Input.GetKey(KeyCode.Space))
            {
                if (charCtr.isGrounded)
                {
                    jumpTime = Time.time + jumpDuration;
                }
                if (jumpTime > Time.time)
                {
                    moveDirection.y += jumpForce*Time.deltaTime;
                }
            }
            moveDirection.y += gravityVector.y*Time.deltaTime;
            charCtr.Move(moveDirection);
            //Rotation
            float rotX = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
            float rotY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
            transform.Rotate(0, rotX, 0);//yes, apparently it's correct to switch the X and Y here
            viewCamera.transform.Rotate(-rotY, 0, 0);
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

    [Command]
    public void CmdFreeze(bool freeze)
    {
        frozen = freeze;
    }
}
