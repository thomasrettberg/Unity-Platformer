using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private SaveGameData savegame;
    private PlayerBehaviour player;
    private CanvasFader canvasFader;
    private bool revertToSaveGame = false;

    private void Awake()
    {
        LoadLastSaveGame();
        canvasFader = FindObjectOfType<CanvasFader>();
        SceneManager.sceneLoaded += WhenSceneWasLoaded;
    }

    private void Start()
    {
        Load();
    }

    private void Update()
    {
       StartCoroutine(HandlePlayerAliveStatus());
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= WhenSceneWasLoaded;
    }

    private void WhenSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (canvasFader != null)
        {
            canvasFader.FadeIn(1f);
        }
    }

    private void Load()
    {
        Debug.Log("Lade Level: " + SaveGameData.current.currentLevel);
        SwitchToScene(SaveGameData.current.currentLevel);
        revertToSaveGame = false;
    }

    public void SwitchToScene(int levelInBuildIndex)
    {
        for (int i = SceneManager.sceneCount - 1; i > 0; i = i - 1)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).name);
        }
        SceneManager.LoadScene(SceneUtility.GetScenePathByBuildIndex(levelInBuildIndex), LoadSceneMode.Additive);
    }

    /// <summary>
    /// Lädt den letzten Speicherpunkt.
    /// </summary>
    private void LoadLastSaveGame()
    {
        SaveGameData.current = SaveGameData.LoadData();
    }

    /// <summary>
    /// Überprüft pro Frame, ob der Spieler noch lebt.
    /// Falls nein, lade vom letzten bekannten Speicherpunkt.
    /// </summary>
    private IEnumerator HandlePlayerAliveStatus()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        if (player == null || player.IsPlayerAlive()) { yield break; }
        player.enabled = false;
        revertToSaveGame = true;
        canvasFader.FadeOut(3f);
        yield return new WaitUntil(() => Input.GetAxis("Continue Game") > 0f);
        RevertToSaveGame(revertToSaveGame);
    }

    /// <summary>
    /// Lädt zum nächsten bekannten Speicherpunkt.
    /// </summary>
    /// <param name="revertToSaveGame">Soll geladen werden, oder nicht?</param>
    private void RevertToSaveGame(bool revertToSaveGame)
    {
        if (revertToSaveGame)
        {
            LoadLastSaveGame();
        }
    }
}
