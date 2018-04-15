using UnityEngine;

public class SavepointBehaviour : MonoBehaviour
{
    /// <summary>
    /// Zeichnungsverhalten im Scene, damit die eigentlich unsichtbaren 
    /// SavePoints zur besseren Verarbeitung sichtbar werden.
    /// </summary>
    private void OnDrawGizmos()
    {
        Utils.DrawBoxCollider(this, Color.black);
    }

    /// <summary>
    /// Triggert das Speichern.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
        if (player != null && player.IsPlayerAlive() && !gameObject.name.Equals(SaveGameData.current.lastTriggeredSavepoint))
        {
            player.SetTriggeredSavepoint(gameObject.name);
            SaveGameData.current.Save();
        }
    }
}
