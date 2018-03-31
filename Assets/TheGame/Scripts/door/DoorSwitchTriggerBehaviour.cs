using UnityEngine;

/// <summary>
/// Realisieirt die Interaktion bei Türen mit Schaltern.
/// </summary>
public class DoorSwitchTriggerBehaviour : MonoBehaviour {

    /// <summary>
    /// Referenz auf das Tür-Objekt, um das Öffnen
    /// zu realisieren.
    /// </summary>
    public Animator doorAnimator;

    /// <summary>
    /// Zeiger auf das MeshRenderer-Objekt, um
    /// unter anderem den Wechsel der Lichtschalter der Console
    /// zu realisieren.
    /// </summary>
    public MeshRenderer consoleMesh;

    private void Awake()
    {
        SaveGameData.OnSave += Saveme;
        SaveGameData.OnLoad += Loadme;
    }

    private void OnDestroy()
    {
        SaveGameData.OnLoad -= Loadme;
        SaveGameData.OnSave -= Saveme;
    }

    private void Start()
    {
        Loadme(SaveGameData.GetCurrentSaveGameData());
    }

    private void Saveme(SaveGameData savegame)
    {
        savegame.isOpenTriggered = doorAnimator.GetBool("isOpenTriggered");
    }

    private void Loadme(SaveGameData savegame)
    {
        if (savegame != null &&  
            gameObject.scene.buildIndex == savegame.currentLevel)
        {
            HandleDoorSwitch(savegame.isOpenTriggered);
        }
    }

    /// <summary>
    /// Zeichnungsverhalten im Scene, damit die eigentlich unsichtbaren 
    /// DoorSwitchTrigger zur besseren Verarbeitung sichtbar werden.
    /// </summary>
    private void OnDrawGizmos()
    {
        Utils.DrawBoxCollider(this, Color.red);
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetAxisRaw("Fire1") != 0f && !doorAnimator.GetBool("isOpenTriggered"))
        {
            HandleDoorSwitch(true);
        }
    }

    public void HandleDoorSwitch(bool isOpenTriggered)
    {
        OpenDoor(isOpenTriggered);
        SwitchLightBulbs();
    }

    private void SwitchLightBulbs()
    {
        Material[] bulbs = consoleMesh.materials;
        Material tempBulb = bulbs[2];
        bulbs[2] = bulbs[1];
        bulbs[1] = tempBulb;
        bulbs[2].color = Color.green;
        consoleMesh.materials = bulbs;
    }

    private void OpenDoor(bool isOpenTriggered)
    {
        doorAnimator.SetBool("isOpenTriggered", isOpenTriggered);
    }
}
