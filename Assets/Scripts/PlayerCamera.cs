using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // For Zooming in and out
    float FOV; // Field of View
    public float maxZm = 80f; // Max zoom
    public float minZm = 15f; // Min zoom
    public float sensitivity = 5f; // Zoom scroll sensitivity
    public int x_cam = -10;
    public int y_cam = 52;		// Starting values
    public int z_cam = -5;
    public int cam_speed = 3;

    // The Camera
    Camera theCam;

    // For moving the camera up, down, left, right
    public Vector2 topLeftOfMap; // Top left corner of the current map
    public Vector2 bottomRightOfMap; // Bottom right corner of the current map

    // Moving using the mouse
    
    // Start is called before the first frame update
    void Start()
    {
        theCam = this.GetComponent<Camera>();
        // Set initial rotation
        this.transform.rotation = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
        // Set initial postion
        this.transform.position = new Vector3(x_cam, y_cam, z_cam);
        // Set intial ZOOM
        FOV = 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Input.mousePosition is (0, 0) to (Screen.width, Screen.height) Use this to move camera w/ mouse
        // Moving using the mouse
        //if(Input.mousePosition.x)

        // Start block for mousewheel to zoom in and out (Do not put in FixedUpdate)
        FOV -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        FOV = Mathf.Clamp(FOV, minZm, maxZm);
        theCam.fieldOfView = FOV;
        // End

    }

    void FixedUpdate(){
        // Moving using the arrow keys
        if(Input.GetKey("w") && this.transform.position.z < topLeftOfMap.y){
            this.transform.Translate(0, cam_speed * Time.fixedDeltaTime, 0); // The reason this one and the next one are swapping y and z ...

        }else if(Input.GetKey("s") && this.transform.position.z > bottomRightOfMap.y){
            this.transform.Translate(0, -cam_speed * Time.fixedDeltaTime, 0); // ... Is because the camera is rotated 90 degrees in start()

        }if(Input.GetKey("a") && this.transform.position.x > topLeftOfMap.x){
            this.transform.Translate(-cam_speed * Time.fixedDeltaTime, 0, 0);

        }else if(Input.GetKey("d") && this.transform.position.x < bottomRightOfMap.x){
            this.transform.Translate(cam_speed * Time.fixedDeltaTime, 0, 0);
        }
    }
}