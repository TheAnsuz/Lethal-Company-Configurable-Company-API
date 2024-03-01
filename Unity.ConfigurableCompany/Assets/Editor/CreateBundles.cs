using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class ExportAssetBundle
{
    public static string OutputDirectory = $"Assets{Path.DirectorySeparatorChar}AssetBundles";

    [MenuItem("Assets/Create bundles")]
    public static void BuildAllAssetBundles()
    {
        Directory.CreateDirectory(OutputDirectory);
        BuildPipeline.BuildAssetBundles(OutputDirectory, BuildAssetBundleOptions.ForceRebuildAssetBundle, EditorUserBuildSettings.activeBuildTarget);

        NotifyWindow window = ScriptableObject.CreateInstance<NotifyWindow>();
        window.titleContent = new GUIContent("Done");
        window.maxSize = new(150, 20);
        window.minSize = new(150, 20);
        window.Show();
    }

    public class NotifyWindow : EditorWindow
    {

        private void CreateGUI()
        {
            var info = new Label("Asset bundles built");

            rootVisualElement.Add(info);
        }
    }
}