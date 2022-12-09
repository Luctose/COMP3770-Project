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
    public int y_cam = 52;
    public int z_cam = -5;
    public int cam_speed = 3;

    // The Camera
    Camera theCam;

    // For moving the camera up, down, left, right
    public Vector2 topLeftOfMap; // Top left corner of the current map
    public Vector2 bottomRightOfMap; // Bottom right corner of the current map

    Vector2 bottomLeftOfMap; // These calculated from what's given
    Vector2 topRightOfMap;
    // Moving using the mouse

    // Moving using the arrow keys
    
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
        
        bottomLeftOfMap.Set(topLeftOfMap.x, bottomRightOfMap.y);
        topRightOfMap.Set(bottomRightOfMap.x, topLeftOfMap.y);
    }

    // Update is called once per frame
    void Update()
    {
        // Input.mousePosition is (0, 0) to (Screen.width, Screen.height) Use this to move camera w/ mouse
        // Moving using the mouse
        //if(Input.mousePosition.x)

        // Moving using the arrow keys
        if(Input.GetKey("w")){
            this.transform.Translate(0, cam_speed * Time.fixedDeltaTime, 0);

        }else if(Input.GetKey("s")){
            this.transform.Translate(0, -cam_speed * Time.fixedDeltaTime, 0);

        }if(Input.GetKey("a")){
            this.transform.Translate(-cam_speed * Time.fixedDeltaTime, 0, 0);

        }else if(Input.GetKey("d")){
            this.transform.Translate(cam_speed * Time.fixedDeltaTime, 0, 0);
        }

        // Start block for mousewheel to zoom in and out
        FOV += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        FOV = Mathf.Clamp(FOV, minZm, maxZm);
        theCam.fieldOfView = FOV;
        // End
    }
}
