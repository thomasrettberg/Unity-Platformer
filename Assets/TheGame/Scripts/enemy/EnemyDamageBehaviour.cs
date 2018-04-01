using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBehaviour : MonoBehaviour {

    [Range(0.0f, 1.0f)]
    [SerializeField] float damageFactor;

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
