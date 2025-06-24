using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static string path = Application.persistentDataPath + "/save.json";

    public static void Save(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static GameSaveData Load()
    {
        if (!File.Exists(path))
            return new GameSaveData();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameSaveData>(json);
    }
}