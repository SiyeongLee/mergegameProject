using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene("St1");
    }

    public void GameEx()
    {
        Application.Quit();
    }


}
