using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private SaveGameData savegame;
    private PlayerBehaviour player;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
    }

    private void Start()
    {
        LoadLastSaveGame();
        Loadme(savegame);
    }

    private void Update()
    {
        HandlePlayerAliveStatus();
    }

    private void OnDestroy()
    {
        SaveGameData.OnSave -= Saveme;
    }

    private void Saveme(SaveGameData savegame)
    {
        if (SceneManager.sceneCount > 1)
        {
            savegame.currentLevel = SceneManager.GetSceneAt(1).buildIndex;
        }
    }

    private void Loadme(SaveGameData savegame)
    {
        SwitchToScene(savegame.currentLevel);
        savegame.TriggerOnLoad();
    }

    public static void SwitchToScene(int levelInBuildIndex)
    {
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1).buildIndex);
        }
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(levelInBuildIndex), LoadSceneMode.Additive);
    }

    /// <summary>
    /// Lädt den letzten Speicherpunkt.
    /// </summary>
    private void LoadLastSaveGame()
    {
        savegame = SaveGameData.LoadData(); 
    }

    /// <summary>
    /// Überprüft pro Frame, ob der Spieler noch lebt.
    /// Falls nein, lade vom letzten bekannten Speicherpunkt.
    /// </summary>
    private void HandlePlayerAliveStatus()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        if (player.IsPlayerAlive()) { return; }
        player.enabled = false;
        Loadme(savegame);
    }
}
