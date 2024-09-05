﻿using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Title,
    Lobby,
    InGame,
    MAX
}
public class SceneLoader : SingletonBehavior<SceneLoader>
{
    public void LoadScene(SceneType sceneType)
    {
        Logger.Log($"{sceneType} scene loading...");

        // 타임 스케일을 초기화 해주기 위해서
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneType.ToString());
    }

    public void ReloadScene()
    {
        Logger.Log($"{SceneManager.GetActiveScene().name} scene loading...");

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
