using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField] float damageFactor;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
    }

    private void Start()
    {
        Loadme(SaveGameData.GetCurrentSaveGameData());
    }

    private void OnDestroy()
    {
        SaveGameData.OnSave -= Saveme;
    }

    private void Saveme(SaveGameData savegame)
    {
        savegame.barrelPosition = transform.position;
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame.barrelPosition != Vector3.zero && savegame != null &&
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            transform.position = savegame.barrelPosition;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandlePlayerCollision(collision);
    }

    /// <summary>
    /// Wenn der Spieler getroffen wird, ziehe ihm
    /// den Schaden ab, welcher im Inspector eingestellt ist.
    /// </summary>
    /// <param name="collision"></param>
    private void HandlePlayerCollision(Collision collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            player.LooseHealth(damageFactor);
        }
    }
}
