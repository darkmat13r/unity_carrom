using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{


    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject exitAlertPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject buttonPause;


    private void Start()
    {
        ShowMainMenu(true);
    }

    public void ShowMainMenu(bool show)
    {
        Time.timeScale = 1;
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(show);
        }
    }

    public void ShowExitAlert(bool show)
    {
        if (exitAlertPanel != null)
        {
            exitAlertPanel.SetActive(show);
        }

        if (buttonPause != null)
        {
            buttonPause.SetActive(true);
        } 
        ShowMainMenu(false);
        ShowPausePanel(false);
        ShowGameOver(false);
       
    }

    


    public void ShowPausePanel(bool show)
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(show);
        }
    }

    public void ShowGameOver(bool show)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
        }
    }


    public void OnPause()
    {
        Time.timeScale = 0;
        if (buttonPause != null)
        {
            buttonPause.SetActive(false);
        }
        ShowPausePanel(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (buttonPause != null)
        {
            buttonPause.SetActive(true);
        }
        ShowPausePanel(false);
    }

    public void ShowScore(string scoreText)
    {
        var panel = gameOverPanel.GetComponent<GameOverPanel>();
        if (panel != null)
        {
            panel.ShowScore(scoreText);
        }
    }
}