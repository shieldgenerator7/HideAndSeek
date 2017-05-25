using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float speed = 3.0f;
    public float rotSpeed = 7.0f;
    public GameObject cameraPrefab;
    private Camera viewCamera;

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
        }
    }
}
