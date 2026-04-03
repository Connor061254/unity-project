using UnityEngine;
using System.IO;

public class TelemetrySender : MonoBehaviour
{
    public string dropFolder = @"G:\My Drive\incoming_data";

    [System.Serializable]
    public class EggData
    {
        public string player_name;
        public string egg_id;
    }

    public void SendEggData(string playerName, string eggId)
    {
        EggData data = new EggData { player_name = playerName, egg_id = eggId };
        string jsonText = JsonUtility.ToJson(data);

        string fileName = "egg_data_" + System.DateTime.Now.Ticks + ".json";
        string fullPath = Path.Combine(dropFolder, fileName);

        File.WriteAllText(fullPath, jsonText);

        Debug.Log("Game dropped a new file for Python at: " + fullPath);
        
        Application.OpenURL(dropFolder);
        Debug.Log("Game dropped a new file for Python at: " + fullPath);
    }
}