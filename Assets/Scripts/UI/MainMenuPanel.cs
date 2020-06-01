using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    public void OnStartGame()
    {
        GameManager.Instance.LoadGame();
    }


    public void OnExitGame()
    {
        uiManager.ShowExitAlert(true);
    }
}
