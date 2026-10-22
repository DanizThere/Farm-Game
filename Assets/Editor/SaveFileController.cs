using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveFileController : MonoBehaviour
{
    [MenuItem("Tools/SaveFile/Delete SaveData")]
    private static void DeleteInventorySave()
    {
        var path = Path.Combine(Application.persistentDataPath, "saves", "GameData.json");
        var pathBak = Path.Combine(Application.persistentDataPath, "saves", "GameData.json.bak");

        if (File.Exists(path))
        {
            Debug.Log("SaveData was deleted");
            File.Delete(path);
            if (File.Exists(pathBak))
            {
                Debug.Log("SaveData bak was deleted");
                File.Delete(pathBak);
                return;
            }
            return;
        }

        Debug.Log("SaveData was already deleted");
    }
}
