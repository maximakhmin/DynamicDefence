using System.Collections.Generic;

public static class DataBaseAdapter
{
    public static float[] getVolume()
    {
        DataStruct data = DataBaseRepository.Load();
        return new float[] { data.musicVolume , data.soundVolume} ;
    }

    public static void setVolume(float musicVolume, float soundVolume)
    {
        DataStruct data = DataBaseRepository.Load();
        data.musicVolume = musicVolume;
        data.soundVolume = soundVolume;
        DataBaseRepository.Save(data);
    }

    public static int getLevelRecord(string level)
    {
        DataStruct data = DataBaseRepository.Load();
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
        DataStruct data = DataBaseRepository.Load();
        return data.levelRecords;
    }

    public static void setLevelRecord(string level, int record)
    {
        DataStruct data = DataBaseRepository.Load();
        if (data.levelRecords.ContainsKey(level))
        {
            data.levelRecords[level] = record;
        }
        else
        {
            data.levelRecords.Add(level, record);
        }
        DataBaseRepository.Save(data);
    }

}
