using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    private SaveGameData savegame;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
    }

    private void Start()
    {
        LoadLastSaveGame();
        Loadme(savegame);
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
}
