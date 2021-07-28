﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    public string sceneName;
    public void OnPlayButtonClick()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
