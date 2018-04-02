using UnityEngine;

public class SavepointBehaviour : MonoBehaviour
{
    /// <summary>
    /// Nutzen der SaveGameData-Komponente via Komposition.
    /// </summary>
    public SaveGameData saveGameData = null;

    /// <summary>
    /// SaveGameData kann in Unity nur über Start oder Awake 
    /// initialisiert werden.
    /// </summary>
    private void Start()
    {
        saveGameData = new SaveGameData();
    }

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
        if (player != null && player.IsPlayerAlive())
        {
            player.SetTriggeredSavepoint(gameObject.name);
            saveGameData.Save();
        }
    }
}
