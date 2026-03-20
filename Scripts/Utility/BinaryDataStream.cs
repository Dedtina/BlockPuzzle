using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
public class BinaryDataStream : MonoBehaviour
{
    public static void Save<T>(T serializedObject, string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path + fileName + ".dat", FileMode.Create);

        try
        {
            formatter.Serialize(stream, serializedObject);
        }
        catch (SerializationException e)
        {
            Debug.Log("Save failed: " + e.Message);
        }
        finally
        {
            stream.Close();
        }

    }

    public static bool Exists(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        string fullFileName = fileName + ".dat";
        return File.Exists(path + fullFileName);
    }

    public static T Read<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path + fileName + ".dat", FileMode.Open);
        T returnType = default(T);

        try
        {
            returnType = (T)formatter.Deserialize(stream);
        }
        catch (SerializationException e)
        {
            Debug.Log("Read failed: " + e.Message);
        }
        finally
        {
            stream.Close();
        }
        return returnType;
    }
}
