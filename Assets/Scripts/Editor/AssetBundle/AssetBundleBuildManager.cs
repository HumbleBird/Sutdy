using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetBundleBuildManager 
{
    [MenuItem("MyTool/AssetBundle Build")]

    public static void AssetBundleBuild()
    {
        string directory = "./Bundle";

        if(!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        EditorUtility.DisplayDialog("에셋 번들 빌드", "에셋 번들 빌드를 완료했습니다.", "완료");
    }
}
