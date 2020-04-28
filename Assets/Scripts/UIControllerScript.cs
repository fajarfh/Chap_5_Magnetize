using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControllerScript : MonoBehaviour
{

    public GameObject pausePanel;
    public GameObject resumeBtn;
    public GameObject levelClearTxt;
    public GameObject mainMenuBtn;

    private Scene currActiveScene;
    private bool isPaused = false;
    private bool isEnded = false;

    public PlayerControl player;

    // FUngsi-fungsi UI
    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pausePanel.SetActive(true);
        levelClearTxt.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        pausePanel.SetActive(false);
    }

    public void RestartGame(string sene)
    {
        Time.timeScale = 1;
        isEnded = false;

        if (sene == "cur")
        {
            SceneManager.LoadScene(currActiveScene.name);
        }
        else
        {
            SceneManager.LoadScene(sene);
        }
        
    }

    public void EndGame()
    {
        isEnded = true;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        levelClearTxt.SetActive(true);
        resumeBtn.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        currActiveScene = SceneManager.GetActiveScene();
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused && !isEnded && !player.isCrashed) 
            { 
                
                PauseGame();

            } else if (isPaused)
            {
                
                ResumeGame();

            }

           
        }
    }
}
