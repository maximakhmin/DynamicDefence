using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class DataStruct
{
    public float musicVolume;
    public float soundVolume;
    public Dictionary<string, int> levelRecords;
}

public static class SaveFileHandler
{ 
    private static string PATH = Application.persistentDataPath + "/save.json";

    public static void Save(DataStruct data)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.None);
        File.WriteAllText(PATH, json);
    }

    public static void CreateNewSaveIfNotExists()
    {
        if (!File.Exists(PATH))
        {
            DataStruct data = new DataStruct();
            data.musicVolume = 0.25f;
            data.soundVolume = 1.0f;
            data.levelRecords = new Dictionary<string, int>();

            string json = JsonConvert.SerializeObject(data, Formatting.None);
            File.WriteAllText(PATH, json);
        }
    }

    public static DataStruct Load()
    {
        if (File.Exists(PATH))
        {
            string json = File.ReadAllText(PATH);
            return JsonConvert.DeserializeObject<DataStruct>(json);
        }
        return null;
    }

}
