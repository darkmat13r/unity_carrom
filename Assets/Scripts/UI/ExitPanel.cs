using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPanel : MonoBehaviour
{

    

    public void OnConfirm()
    {
        GameManager.Instance.ExitGame();
    }


    public void OnCancel()
    {
        gameObject.SetActive(false);
    }
}
