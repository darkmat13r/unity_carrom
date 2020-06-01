using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Board;
using Pieces;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance => _instance;

  

    // Start is called before the first frame update
    void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
        }else if (_instance != null)
        {
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        Debug.Log("on Start TIme " + Time.timeScale);
    }

    


    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
        StartCoroutine(UnloadMainMenu());
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        StartCoroutine(UnloadMainGame());
    }

    IEnumerator UnloadMainGame()
    {
        yield return null;
        try
        {
            SceneManager.UnloadSceneAsync("MainGame");
        }
        catch (Exception e)
        {
            
        }
    }
    IEnumerator UnloadMainMenu()
    {
        yield return null;
        try
        {
            SceneManager.UnloadSceneAsync("MainMenu");
        }
        catch (Exception e)
        {
            
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}