using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Font menuFont;
    GUIStyle noBackground;
    GUIStyle sliderText;

    int screenH;
    int screenW;

    int screen;

    private int m_buttonW; //200
    private int m_buttonH; //50
    public int m_nbButtons = 4;
    Rect[] m_buttonsRect;
    Rect[] load_buttonsRect;
    Rect settingsButton;
    Rect audioSlider;

    string[] m_levelNames;
    string[] load_levelNames;

    GameObject slider;
    
    // Start is called before the first frame update
    void Start()
    {
        screen = 0;
        screenH = Screen.width;
        screenW = Screen.height;

        m_buttonW = screenW/4;
        m_buttonH = screenH/14;

        noBackground = new GUIStyle();
        noBackground.alignment = TextAnchor.MiddleLeft;
        noBackground.fontSize = m_buttonH - m_buttonH/6;
        noBackground.normal.textColor = Color.white;
        noBackground.font = menuFont;

        sliderText = new GUIStyle();
        sliderText.alignment = TextAnchor.MiddleLeft;
        sliderText.fontSize = m_buttonH/4;
        sliderText.normal.textColor = Color.white;
        sliderText.font = menuFont;

        slider = GameObject.FindWithTag("SettingsPage");
        slider.SetActive(false);

        // =====   MENU BUTTONS   ===== //
        m_buttonsRect = new Rect[m_nbButtons];
        for(int i = 0; i < m_nbButtons; ++i){
            m_buttonsRect[i] = new Rect(m_buttonW/6, (i * (m_buttonH + m_buttonH/8)) + screenH/16, m_buttonW, m_buttonH);
        }

        m_levelNames = new string[m_nbButtons];
        m_levelNames[0] = "New Game";
        m_levelNames[1] = "Load Game";
        m_levelNames[2] = "Settings";
        m_levelNames[3] = "Quit";

        // =====   LOAD BUTTONS   ===== //
        load_buttonsRect = new Rect[m_nbButtons];
        for(int i = 0; i < m_nbButtons; ++i){
            load_buttonsRect[i] = new Rect(m_buttonW/6, (i * (m_buttonH + m_buttonH/8)) + screenH/16, m_buttonW, m_buttonH);
        }

        load_levelNames = new string[m_nbButtons];
        load_levelNames[0] = "Save 1";
        load_levelNames[1] = "Save 2";
        load_levelNames[2] = "Save 3";
        load_levelNames[3] = "Back";

        settingsButton = new Rect(m_buttonW/6, (3 * (m_buttonH + m_buttonH/8)) + screenH/16, m_buttonW, m_buttonH);
        audioSlider = new Rect(m_buttonW/6, (2 * (m_buttonH + m_buttonH/8)) + screenH/16, m_buttonW, m_buttonH);

        AudioListener.volume = PlayerPrefs.GetFloat("volumeFloat", 1f);
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
                    else if(i == 1){screen=1;}
                    else if(i == 2){screen=2;}
                    else if(i == 0){SceneManager.LoadScene("LevelMerge");}
                }
            }
        }
        else if (screen == 1)
        {
            for(i = 0; i < m_nbButtons; ++i){
                if(GUI.Button(load_buttonsRect[i], load_levelNames[i], noBackground)){
                    // When back is pressed
                    if(i == 3){screen=0;}
                    else{
                        // Load the pressed scene
                        // Write data to the data file corresponding with selected save
                    }
                }
            }
        }
        else if (screen == 2)
        {
            slider.SetActive(true);
            if(GUI.Button(settingsButton, "Back", noBackground)){PlayerPrefs.Save();slider.SetActive(false);screen=0;}
        }
    }
}
