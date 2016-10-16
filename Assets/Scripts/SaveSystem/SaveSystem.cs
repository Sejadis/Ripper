using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    public static bool Save(Save save, string name, SaveType type)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(GetSavePath(name, type), FileMode.Create))
        {
            try
            {
                formatter.Serialize(stream, save);
            }
            catch (Exception e)
            {
                Debug.Log("Saving Exception: " + e.Message);
                return false;
            }
        }

        return true;
    }

    public static Save Load(string name, SaveType type)
    {
        if (!DoesSaveExist(name, type))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();

        using (FileStream stream = new FileStream(GetSavePath(name, type), FileMode.Open))
        {
            try
            {
                return formatter.Deserialize(stream) as Save;
            }
            catch (Exception e)
            {
                Debug.Log("Loading Exception: " + e.Message);
                return null;
            }
        }
    }

    public static bool DeleteSave(string name, SaveType type)
    {
        try
        {
            File.Delete(GetSavePath(name, type));
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public static bool DoesSaveExist(string name, SaveType type)
    {
        return File.Exists(GetSavePath(name, type));
    }

    private static string GetSavePath(string name, SaveType type)
    {
        return Path.Combine(Application.persistentDataPath, name + "." + type.ToString());
    }
}

