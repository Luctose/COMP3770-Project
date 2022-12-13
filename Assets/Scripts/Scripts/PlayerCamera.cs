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
    // Initialize camera position
    public int x_cam = -10;
    public int y_cam = 52;
    public int z_cam = -5;
    // Camera slide speed
    public int cam_speed = 3;

    // The Camera
    Camera theCam;

    // For moving the camera up, down, left, right
    public Vector2 topLeftOfMap; // Top left corner of the current map
    public Vector2 bottomRightOfMap; // Bottom right corner of the current map

    // Moving using the mouse
    Vector3 cam_pos; // Used to modify camera position
    bool lockMouseSlide = false; // Used to lock the camera for mouse sliding
    int edgeRatio = Screen.width / 10; // 10% of the screen width

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

        cam_pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Start block for mousewheel to zoom in and out (Do not put in FixedUpdate)
        FOV -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        FOV = Mathf.Clamp(FOV, minZm, maxZm);
        theCam.fieldOfView = FOV;
        // End

        // Lock/Unlock camera for mouse on key "y"
        if(Input.GetKeyDown("y")){
            lockMouseSlide = !lockMouseSlide;
        }

        // Make cam_pos unable to exceed bounds
        cam_pos.x = Mathf.Clamp(cam_pos.x, topLeftOfMap.x, bottomRightOfMap.x);
        cam_pos.z = Mathf.Clamp(cam_pos.z, bottomRightOfMap.y, topLeftOfMap.y);
        // Update camera position
        this.transform.position = new Vector3(cam_pos.x, cam_pos.y, cam_pos.z);
    }

    void FixedUpdate(){
        // Moving using the arrow keys
        if(Input.GetKey("w")){
            cam_pos.z += cam_speed * Time.fixedDeltaTime;

        }else if(Input.GetKey("s")){
            cam_pos.z -= cam_speed * Time.fixedDeltaTime;

        }if(Input.GetKey("a")){
            cam_pos.x -= cam_speed * Time.fixedDeltaTime;

        }else if(Input.GetKey("d")){
            cam_pos.x += cam_speed * Time.fixedDeltaTime;
        }


        // Moving using mouse on the edge of the screen
        // First check if camera is locked
        if(!lockMouseSlide){
            // Mouse close to right
            if(Input.mousePosition.x > Screen.width - edgeRatio){
                cam_pos.x += cam_speed * Time.fixedDeltaTime;
            }
            // Mouse close to left
            else if(Input.mousePosition.x < edgeRatio){
                cam_pos.x -= cam_speed * Time.fixedDeltaTime;
            }
            // Mouse close to top
            if(Input.mousePosition.y > Screen.height - edgeRatio){
                cam_pos.z += cam_speed * Time.fixedDeltaTime;
            }
            // Mouse close to bottom
            else if(Input.mousePosition.y < edgeRatio){
                cam_pos.z -= cam_speed * Time.fixedDeltaTime;
            }
        }
    }
}