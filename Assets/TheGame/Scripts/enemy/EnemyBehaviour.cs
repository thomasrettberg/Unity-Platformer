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
    }

    private void Start()
    {
        Loadme(SaveGameData.GetCurrentSaveGameData());
        rig = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        SaveGameData.OnSave -= Saveme;
    }

    private void Saveme(SaveGameData savegame)
    {
        savegame.enemyPosition = transform.position;
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame != null && savegame.enemyPosition != Vector3.zero &&
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            transform.position = savegame.enemyPosition;
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
            float damage = Mathf.Clamp(damageFactor * rig.velocity.magnitude, 0.0f, damageFactor);
            player.LooseHealth(damage);
        }
    }
}
