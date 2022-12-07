using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Font menuFont;
    GUIStyle noBackground;

    int screenH;
    int screenW;

    int screen = 0;

    private int m_buttonW; //200
    private int m_buttonH; //50
    public int m_nbButtons = 4;
    Rect[] m_buttonsRect;

    string[] m_levelNames;
    
    // Start is called before the first frame update
    void Start()
    {
        screenH = Screen.width;
        screenW = Screen.height;

        m_buttonW = screenW/4;
        m_buttonH = screenH/14;

        noBackground = new GUIStyle();
        noBackground.alignment = TextAnchor.MiddleLeft;
        noBackground.fontSize = m_buttonH - m_buttonH/6;
        noBackground.normal.textColor = Color.white;
        noBackground.font = menuFont;

        m_buttonsRect = new Rect[m_nbButtons];
        for(int i = 0; i < m_nbButtons; ++i){
            m_buttonsRect[i] = new Rect(m_buttonW/6, (i * (m_buttonH + m_buttonH/8)) + screenH/16, m_buttonW, m_buttonH);
        }

        m_levelNames = new string[m_nbButtons];
        m_levelNames[0] = "New Game";
        m_levelNames[1] = "Load Game";
        m_levelNames[2] = "Settings";
        m_levelNames[3] = "Quit";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI(){
        int i = 0;
        if (screen == 0)
        {
            for(i = 0; i < m_nbButtons; ++i){
                if(GUI.Button(m_buttonsRect[i], m_levelNames[i], noBackground)){
                    // When "Quit" is pressed
                    if(i == 3){
                        // Put cleanup code before game stops execution here


                        // Quit the game (Ignored in the unity editor only when the game is built)
                        Application.Quit();
                    }
                    else{
                        // Load the pressed scene
                        SceneManager.LoadScene(m_levelNames[i]);
                    }
                }
            }
        }
        if (screen == 1)
        {}
    }
}
