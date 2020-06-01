using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class PausePanel : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    public void OnMainMenu()
    {
        GameManager.Instance.LoadMenu();
    }

    public void OnExitGame()
    {
        uiManager.ShowExitAlert(true);
    }

    public  void OnResume()
    {
        uiManager.ResumeGame();
    }
}
