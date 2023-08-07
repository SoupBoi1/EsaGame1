using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManage : MonoBehaviour
{
    public InputActionAsset actionsAssests;
    InputActionMap UtilityMap;
    InputAction pauseAction;
    InputAction jumpAction;

    bool pausepress = false;

    public GameObject UI;

    private void Awake()
    {
        UI.active = false;

    }

    /**
    *pauses the game
    *
    **/
    public void PauseingGame()
    {
        print("pasusepuress");
        if (!pausepress)
        {
            PauseGame();
            pausepress = true;
        }
        else {
            ReumeGame();
            pausepress = false;
        }
      
    }

    /**
     *pauses the game
     *
     **/
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        UI.active = true;
        Time.timeScale = 0;
    }
    /**
     * resumes the game
     * 
     **/
    public void ReumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UI.active = false;
        Time.timeScale = 1; 
    }
}
