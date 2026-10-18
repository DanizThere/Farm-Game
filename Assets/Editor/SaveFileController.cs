using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveFileController : MonoBehaviour
{
    [MenuItem("Tools/SaveFile/Delete InventorySave")]
    private static void DeleteInventorySave()
    {
        var path = Path.Combine(Application.persistentDataPath, "saves", "data.json");

        if (File.Exists(path))
        {
            Debug.Log("SaveData was deleted");
            File.Delete(path);
            return;
        }

        Debug.Log("SaveData was already deleted");
    }
}
