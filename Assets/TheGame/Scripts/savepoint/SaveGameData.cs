using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SaveGameData {

    public Vector3 playerPosition = Vector3.zero;
    public int currentLevel = 1;
    public float playerHealth = 1f;
    public string lastTriggeredSavepoint = "";
    public List<SaveObject> saveObject = new List<SaveObject>();

    public static string dataName = "savegame.xml";
    public static SaveGameData current = new SaveGameData();

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
        SaveGameData save = new SaveGameData();
        string fileName = GetFilename(dataName);
        if (File.Exists(fileName))
        {
            save = XmlUtils.Load<SaveGameData>(File.ReadAllText(fileName));
        }
        if (OnLoad != null) OnLoad(save);
        return save;
    }

    private static string GetFilename(string dataName)
    {
        return Application.persistentDataPath + System.IO.Path.DirectorySeparatorChar + dataName;
    }

    public SaveObject FindObjectById(string id)
    {
        SaveObject saveObject = current.saveObject.Find(
                    delegate (SaveObject so)
                    {
                        return so.Id == id;
                    }
                );
        return saveObject;
    }
}
