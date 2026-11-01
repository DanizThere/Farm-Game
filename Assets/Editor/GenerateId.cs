using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

public class GenerateId : MonoBehaviour
{
    [MenuItem("Tools/Generate Id")]
    private static void GenerateIds()
    {
        print($"{Guid.NewGuid()}");
    }
}
