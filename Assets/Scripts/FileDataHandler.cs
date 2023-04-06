using System.IO;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class FileDataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        _dataDirPath = dataDirPath;
        _dataFileName = dataFileName;
    }

    public GameData Load()
    {
        // use Path.Combine to account for different OS having different separators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        GameData loadedData = null;
        if(File.Exists(fullPath)) 
        {
            try
            {
                // Load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream)) 
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // deserialize the data from the Json back into the C# object
                //loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                loadedData = JsonConvert.DeserializeObject<GameData>(dataToLoad);

            }
            catch (Exception e)
            {
                Debug.LogError("Error Loading Data from file : " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save (GameData data)
    {
        // use Path.Combine to account for different OS having different separators
        string fullPath = Path.Combine(_dataDirPath, _dataFileName);
        try
        {
            // Create the directory the file will be written on if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream)) 
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file : " + fullPath + '\n' + e);
        }
    }
}
