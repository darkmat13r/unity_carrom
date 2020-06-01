using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreText;
        
    public void OnRestart()
    {
        GameManager.Instance.LoadGame();
    }

    public void OnMainMenu()
    {
        
        GameManager.Instance.LoadMenu();
    }



    public void ShowScore(String score)
    {
        scoreText.text = score;
    }
    
    
}
