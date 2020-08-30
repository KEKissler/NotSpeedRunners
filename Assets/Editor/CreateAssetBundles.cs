using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build Web Build AssetBundles")]
    static void BuildAllAssetBundlesForWebBuilds()
    {
        var bundles = new AssetBundleBuild[1];
        var paths = new List<string>();
        foreach (var guid in AssetDatabase.FindAssets("t:scene", new[] {"Assets/Scenes/LevelsAssetBundle"}))
        {
            paths.Add(AssetDatabase.GUIDToAssetPath(guid));
        }

        bundles[0].assetBundleName = "levels";
        bundles[0].assetNames = paths.ToArray();
        
        var assetBundleDirectory = "Build/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, bundles, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.WebGL);
    }
}