﻿using UnityEngine;

public class MenuManager : MonoBehaviour {

    private Canvas canvas;
    private bool isInputTriggered = false;
    private LevelManager levelManager;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    private void Update ()
    {
        OpenOrCloseMenu();
    }

    public void OnMenuButtonNewPressed()
    {
        if (canvas.enabled)
        {
            SaveGameData.current = new SaveGameData();
            levelManager.SwitchToScene(1);
            canvas.enabled = false;
            PauseOrContinueGame();
        }
    }

    public void OnMenuButtonQuitPressed()
    {
        Debug.Log("Spiel beenden....");
        Application.Quit();
    }

    /// <summary>
    /// Pausiert oder startet das Szenen-Level,
    /// wenn das Spielmenü ein oder ausgeblendet ist.
    /// </summary>
    private void PauseOrContinueGame()
    {
        Time.timeScale = canvas.enabled ? 0f : 1f;
    }

    /// <summary>
    /// Öffnet oder schließt das Spielmenü.
    /// </summary>
    private void OpenOrCloseMenu()
    {
        if (Input.GetAxisRaw("Menu") > 0f)
        {
            if (!isInputTriggered)
            {
                canvas.enabled = !canvas.enabled;
                PauseOrContinueGame();
            }
            isInputTriggered = true;
        }
        else
        {
            isInputTriggered = false;
        }
    }
}
