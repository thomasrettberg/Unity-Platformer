using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrbBehaviour : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField] float newHealth;

    [SerializeField] bool isActive = true;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
        SaveGameData.OnLoad += Loadme;
    }

    private void Start()
    {
        Loadme(SaveGameData.current);
        gameObject.SetActive(isActive);
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame != null &&
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            isActive = savegame.FindObjectById(gameObject.name) == null;
        }
    }

    private void Saveme(SaveGameData savegame)
    {
        if (savegame.FindObjectById(gameObject.name) == null && !isActive)
        {
            SaveObject saveObject = new SaveObject();
            saveObject.Id = gameObject.name;
            saveObject.Tag = gameObject.tag;
            savegame.saveObject.Add(saveObject);
        }
    }

    private void OnDestroy()
    {
        SaveGameData.OnLoad -= Loadme;
        SaveGameData.OnSave -= Saveme;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.AddHealth(newHealth);
            isActive = false;
            gameObject.SetActive(isActive);
        }
    }
}
