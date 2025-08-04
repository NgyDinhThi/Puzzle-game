using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class BinaryDataStream 
{
    public static void Save<T>(T serializedObject, string fileName)
    {
        string path = Application.persistentDataPath + "/save/";

        Directory.CreateDirectory(path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".data", FileMode.Create);
        Debug.Log("Đang lưu file tại: " + path);

        try
        {
            formatter.Serialize(fileStream, serializedObject);

        }
        catch (SerializationException e) 
        { 
            Debug.Log("Lưu file: "+ e.Message);
        }
        finally
        {
            fileStream.Close();
        }
    }   
    
    public static bool Exist(string fileName)
    {
        string path = Application.persistentDataPath + "/save/";
        string fullFileName = fileName + ".data";
        return File.Exists(path + fullFileName);
    }

    public static T Read<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/save/";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".data", FileMode.Open);
        T returnType = default(T);

        try
        {
            returnType = (T)formatter.Deserialize(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.Log("Đọc thất bại. Lỗi: " + e.Message);
        }
        finally
        {
            fileStream.Close();
        }

        return returnType;
    }
}
