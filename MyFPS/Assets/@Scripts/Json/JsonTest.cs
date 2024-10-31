using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Data
{
    public string Name;
    public float Height;
    [JsonProperty]
    private string secret;

    public Data(string name, float height, string secret)
    {
        Name = name;
        Height = height;
        this.secret = secret;
    }

    public override string ToString()
    {
        return Name + " " + Height + " " + secret;
    }
}


public class JsonTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Data charles = new Data("철수", 198, "발바닥에 점이 두개");

        string json1 = JsonConvert.SerializeObject(charles);
        Debug.Log(json1);

        Data aMan = JsonConvert.DeserializeObject<Data>(json1);
        Debug.Log(aMan);
        Save(aMan, "save.txt");

        Data secondMan = Load<Data>("save.txt");
        Debug.Log(secondMan);
    }

    void Save<T>(T data, string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(path);

        try
        {
            string json = JsonConvert.SerializeObject(data);
            json = SimpleEncryptionUtility.Encrypt(json);
            File.WriteAllText(path, json);
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    T Load<T>(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath,filename);

        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                json = SimpleEncryptionUtility.Decrypt(json);
                return JsonConvert.DeserializeObject<T>(json);
            }
            else
            {
                //파일이 없을 때
                return default;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            return default;
        }
    }
}