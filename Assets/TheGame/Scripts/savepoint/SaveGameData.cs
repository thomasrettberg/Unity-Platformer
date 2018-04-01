using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveGameData {

    public Vector3 playerPosition = Vector3.zero;
    public bool isOpenTriggered = false;
    public int currentLevel = 1;
    public float playerHealth = 1f;

    public static string dataName = "savegame.xml";
    private static SaveGameData current;

    public delegate void SaveHandler(SaveGameData savegame);
    public static event SaveHandler OnSave;
    public static event SaveHandler OnLoad;

    /// <summary>
    /// Default-Konstruktor, welcher für die 
    /// Serialisierbarkeit benötigt wird.
    /// </summary>
    public SaveGameData()
    {

    }

    /// <summary>
    /// Speichert einen Spielstand.
    /// </summary>
    public void Save()
    {
        if(OnSave != null)
        {
            OnSave(this);
        }
        File.WriteAllText(GetFilename(dataName), XmlUtils.SaveXmlString(this));
    }

    /// <summary>
    /// Lädt einen Spielstand.
    /// </summary>
    public static SaveGameData LoadData()
    {
        SaveGameData savegame = new SaveGameData();
        string fileName = GetFilename(dataName);
        if (File.Exists(fileName))
        {
            savegame = XmlUtils.Load<SaveGameData>(File.ReadAllText(GetFilename(dataName)));
            current = savegame;
        }
        
        return savegame;
    }

    public void TriggerOnLoad()
    {
        if (OnLoad != null)
        {
            OnLoad(this);
        }
    }

    private static string GetFilename(string dataName)
    {
        return Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + dataName;
    }

    public static SaveGameData GetCurrentSaveGameData()
    {
        return current;
    }

    public static void SetCurrentSaveGameData(SaveGameData savegame)
    {
        current = savegame;
    }
}
