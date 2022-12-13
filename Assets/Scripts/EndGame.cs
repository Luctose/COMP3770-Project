using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{   
    public Font menuFont;
    public Texture image;
    GUIStyle endgametext;
    GUIContent endtext;
    // Start is called before the first frame update
    void Start()
    {
        endgametext = new GUIStyle();
        endgametext.alignment = TextAnchor.MiddleCenter;
        endgametext.fontSize = 50 - 50/6;
        endgametext.normal.textColor = Color.black;
        endgametext.font = menuFont;
        endtext = new GUIContent(image);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI(){
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), endtext, endgametext);
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "Thanks for playing!", endgametext);
    }
}
