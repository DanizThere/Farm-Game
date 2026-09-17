using UnityEngine;
using UnityEditor;
using System.IO;

public class SaveFileController : MonoBehaviour
{
    [MenuItem("Tools/SaveFile/Delete InventorySave")]
    private static void DeleteInventorySave()
    {
        var path = Path.Combine(Application.persistentDataPath, "saves", "InventoryData");

        if (File.Exists(path))
        {
            Debug.Log("InventoryData was deleted");
            File.Delete(path);
            return;
        }

        Debug.Log("InventoryData was already deleted");
    }
}
