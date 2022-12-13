using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject mainPauseMenu;
    public GameObject overlay;
    GameObject currScene;
    int pausedFlag;

    // Start is called before the first frame update
    void Start()
    {
        pausedFlag = 0;
        overlay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (pausedFlag == 0)
        {
            if(Input.GetKeyUp("escape"))
            {
                overlay.SetActive(false);
                pauseCanvas.SetActive(true);
                currScene = mainPauseMenu;
                currScene.SetActive(true);
                pausedFlag = 1;
            }
        }
        else
        {
            if(Input.GetKeyUp("escape"))
            {
                resumeGame();
            }
        }
    }

    public void swapCurrPage(GameObject targPage)
    {
        currScene = targPage;
    }

    public void resumeGame()
    {
        currScene.SetActive(false);
        currScene = null;
        pauseCanvas.SetActive(false);
        overlay.SetActive(true);
        pausedFlag = 0;
    }
}
