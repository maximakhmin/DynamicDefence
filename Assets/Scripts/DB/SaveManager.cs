using System.Collections.Generic;

public static class SaveManager
{
    public static float[] getVolume()
    {
        DataStruct data = SaveFileHandler.Load();
        return new float[] { data.musicVolume , data.soundVolume} ;
    }

    public static void setVolume(float musicVolume, float soundVolume)
    {
        DataStruct data = SaveFileHandler.Load();
        data.musicVolume = musicVolume;
        data.soundVolume = soundVolume;
        SaveFileHandler.Save(data);
    }

    public static int getLevelRecord(string level)
    {
        DataStruct data = SaveFileHandler.Load();
        if (data.levelRecords.ContainsKey(level))
        {
            return data.levelRecords[level];
        }
        else
        {
            return -1;
        }
    }

    public static Dictionary<string, int> getLevelRecords()
    {
        DataStruct data = SaveFileHandler.Load();
        return data.levelRecords;
    }

    public static void setLevelRecord(string level, int record)
    {
        DataStruct data = SaveFileHandler.Load();
        if (data.levelRecords.ContainsKey(level))
        {
            data.levelRecords[level] = record;
        }
        else
        {
            data.levelRecords.Add(level, record);
        }
        SaveFileHandler.Save(data);
    }

}
