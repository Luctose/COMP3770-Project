using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject mainPauseMenu;
    public GameObject currScene;
    int pausedFlag;

    // Start is called before the first frame update
    void Start()
    {
        pausedFlag = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (pausedFlag == 0)
        {
            if(Input.GetKeyUp("escape"))
            {
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
                currScene.SetActive(false);
                currScene = null;
                pauseCanvas.SetActive(false);
                pausedFlag = 0;
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
        pausedFlag = 0;
    }
}
