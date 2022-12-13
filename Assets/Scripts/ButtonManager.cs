using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    GameObject manager;
    PauseMenuManager managerScript;

    public void pageSwap(GameObject targPage)
    {  
        manager = GameObject.FindWithTag("PauseMenu");
        managerScript = manager.GetComponent<PauseMenuManager>();
        targPage.SetActive(true);
        managerScript.swapCurrPage(targPage);
        gameObject.SetActive(false);
    }

    public void menuSwap(GameObject targPage)
    {
        targPage.SetActive(true);
        gameObject.SetActive(false);
    }

    public void quit(){SceneManager.LoadScene("Main Menu");}

    public void saveGame(){//CODE FOR SAVING DATA
    }

    public void returnToGame()
    {
        manager = GameObject.FindWithTag("PauseMenu");
        managerScript = manager.GetComponent<PauseMenuManager>();
        managerScript.resumeGame();
    }

    public void playGame(){SceneManager.LoadScene("LevelMerge");}

    public void appQuit(){Application.Quit();}

    public void saveSettings(){PlayerPrefs.Save();}

}
