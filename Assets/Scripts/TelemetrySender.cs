using UnityEngine;
using System.IO; // This allows us to create and save files!

public class TelemetrySender : MonoBehaviour
{
    // This is the exact folder path from your screenshot! 
    // Unity will drop the files right here so Python can see them.
    // The '@' before the quotes is important for Unity paths!
    public string dropFolder = @"G:\My Drive\incoming_data";

    // 1. We create a little class to format our data nicely for Python
    [System.Serializable]
    public class EggData
    {
        public string player_name;
        public string egg_id;
    }

    // Call this function when the player touches the Easter Egg!
    // Example: SendEggData("Gamer123", "Blue_Egg");
    public void SendEggData(string playerName, string eggId)
    {
        // 2. Package the variables into JSON format
        EggData data = new EggData { player_name = playerName, egg_id = eggId };
        string jsonText = JsonUtility.ToJson(data);

        // 3. Create a unique file name using the exact time 
        // (So if 2 people find an egg at the same time, the files don't overwrite each other)
        string fileName = "egg_data_" + System.DateTime.Now.Ticks + ".json";
        string fullPath = Path.Combine(dropFolder, fileName);

        // 4. Save the file to your computer!
        File.WriteAllText(fullPath, jsonText);

        Debug.Log("Game dropped a new file for Python at: " + fullPath);
        
        // ADD THIS NEW LINE:
        Application.OpenURL(dropFolder);
        Debug.Log("Game dropped a new file for Python at: " + fullPath);
    }
}