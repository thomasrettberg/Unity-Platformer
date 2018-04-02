using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthOrbBehaviour : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField] float newHealth;

    [SerializeField] bool isActive;

    private string id;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
        id = gameObject.name;
    }

    private void Start()
    {
        Loadme(SaveGameData.GetCurrentSaveGameData());
        gameObject.SetActive(isActive);
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame != null &&
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            isActive = !savegame.disabledHealtOrbIds.Contains(id);
        }
    }

    private void Saveme(SaveGameData savegame)
    {
        if (id != null && !gameObject.activeSelf && !savegame.disabledHealtOrbIds.Contains(id))
        {
            savegame.disabledHealtOrbIds.Add(id);
        }
    }

    private void OnDestroy()
    {
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
