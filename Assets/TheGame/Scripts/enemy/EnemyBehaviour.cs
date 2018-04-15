using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField] float damageFactor;
    private Rigidbody rig;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
        SaveGameData.OnLoad += Loadme;
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        Loadme(SaveGameData.current);
    }

    private void OnDestroy()
    {
        SaveGameData.OnLoad -= Loadme;
        SaveGameData.OnSave -= Saveme;
    }

    private void Saveme(SaveGameData savegame)
    {
        SaveObject saveObject = savegame.FindObjectById(gameObject.name);
        if (saveObject == null)
        {
            saveObject = new SaveObject();
            UpdateSaveObject(saveObject);
            savegame.saveObject.Add(saveObject);
        }
        else
        {
            UpdateSaveObject(saveObject);
        }
    }

    private void UpdateSaveObject(SaveObject saveObject)
    {
        saveObject.Id = gameObject.name;
        saveObject.Tag = gameObject.tag;
        saveObject.Position = transform.position;
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame != null &&
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            SaveObject saveObject = savegame.FindObjectById(gameObject.name);
            switch ((saveObject != null) ? saveObject.Tag : "")
            {
                case "barrel":
                    transform.position = saveObject.Position;
                    break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandlePlayerCollision(collision);
    }

    /// <summary>
    /// Wenn der Spieler getroffen wird, ziehe ihm
    /// den Schaden ab, welcher im Inspector eingestellt ist.
    /// Der Schaden richtet sich nach der Geschwindigkeit, kann aber niemals
    /// den Schaden aus dem Inspector überschreiten.
    /// </summary>
    /// <param name="collision">Collision-Objekt</param>
    private void HandlePlayerCollision(Collision collision)
    {
        PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (player != null)
        {
            float damage = Mathf.Clamp(damageFactor * rig.velocity.magnitude, 0.0f, damageFactor);
            player.LooseHealth(damage);
        }
    }
}
