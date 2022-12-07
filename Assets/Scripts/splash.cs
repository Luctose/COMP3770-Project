using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splash : MonoBehaviour
{
    GUIStyle style = new GUIStyle();
    // Start is called before the first frame update
    void Start()
    {
        style.fontSize = 50;
        style.alignment = TextAnchor.MiddleCenter;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey){
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnGUI(){
        GUI.Label(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 100, 200, 200), "Press any button", style);
    }
}
