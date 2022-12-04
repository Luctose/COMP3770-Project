using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour
{
    const int m_buttonW = 200;
    const int m_buttonH = 50;
    public int m_nbButtons = 4;
    Rect[] m_buttonsRect;

    string[] m_levelNames;
    
    // Start is called before the first frame update
    void Start()
    {
        m_buttonsRect = new Rect[m_nbButtons];
        for(int i = 0; i < m_nbButtons; ++i){
            m_buttonsRect[i] = new Rect(500, (i * (m_buttonH + 25)) + 200, m_buttonW, m_buttonH);
        }

        m_levelNames = new string[m_nbButtons];
        m_levelNames[0] = "File 1";
        m_levelNames[1] = "File 2";
        m_levelNames[2] = "File 3";
        m_levelNames[3] = "Back";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI(){
        int i = 0;
        for(i = 0; i < m_nbButtons; ++i){
            if(GUI.Button(m_buttonsRect[i], m_levelNames[i])){
                // If "Back" GUI is clicked
                if(i == 3){
                    SceneManager.LoadScene("MainMenu");
                }
                // If A file is chosen
                else{
                    // WIP: Load chosen save file here with it's current progress
                }

            }
        }
    }
}