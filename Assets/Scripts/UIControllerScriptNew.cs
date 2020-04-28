using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerScriptNew : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject resumeBtn;
    public GameObject levelClearTxt;
    public GameObject nextLvlBtn;
    public GameObject mainMenuBtn;

    private Scene currActiveScene;
    private bool isPaused = false;
    private bool isEnded = false;

    public PlayerControlNew player;

    // FUngsi-fungsi UI
    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        pausePanel.SetActive(true);
        levelClearTxt.SetActive(false);
        nextLvlBtn.SetActive(false);
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
        if(!player.isCrashed)
        { 
            isEnded = true;
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            levelClearTxt.SetActive(true);

            if (currActiveScene.name == "Level3")
            {
                levelClearTxt.GetComponent<Text>().text = "THATS ALL FOLKS!";
            }

            resumeBtn.SetActive(false);
            
            if (currActiveScene.name == "Level3")
            {
                nextLvlBtn.SetActive(false);
            } else
            {

                nextLvlBtn.SetActive(true);

            }
        } else
        {
            player.RestartPosition();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currActiveScene = SceneManager.GetActiveScene();
        player = GameObject.Find("Player").GetComponent<PlayerControlNew>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (currActiveScene.name != "Main"))
        {
            if (!isPaused && !isEnded && !player.isCrashed)
            {

                PauseGame();

            }
            else if (isPaused)
            {

                ResumeGame();

            }


        }
    }
}
