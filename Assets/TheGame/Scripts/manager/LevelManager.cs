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
        canvasFader = FindObjectOfType<CanvasFader>();
        SaveGameData.OnSave += Saveme;
        SceneManager.sceneLoaded += WhenSceneWasLoaded;
    }

    private void Start()
    {
        RevertToSaveGame(true);
    }

    private void Update()
    {
       StartCoroutine(HandlePlayerAliveStatus());
    }

    private void OnDestroy()
    {
        SaveGameData.OnSave -= Saveme;
        SceneManager.sceneLoaded -= WhenSceneWasLoaded;
    }

    private void WhenSceneWasLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (canvasFader != null)
        {
            canvasFader.FadeIn(1f);
        }
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
        revertToSaveGame = false;
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
    private IEnumerator HandlePlayerAliveStatus()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        if (player.IsPlayerAlive()) { yield break; }
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
            Loadme(savegame);
        }
    }
}
