using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace UDialogsEditor
{
    internal class UDialogsMenu 
    {
        private static string GetPackagePath() => Path.GetFullPath("Packages/com.pedro15.udialogs/PackageResources");

        [MenuItem("Tools/UDialogs/Import Essentials")]
        private static void InstallEssentials()
        {
            AssetDatabase.ImportPackage(GetPackagePath() + "/Essentials.unitypackage", false);
        }
    }
}