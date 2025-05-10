using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AddBoxColliders : EditorWindow
{
    [MenuItem("Tools/Add BoxColliders to stage01 assets")]
    public static void AddColliders()
    {
        string path = "Assets/stage01_assets_small";
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { path });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (instance != null)
            {
                var mesh = instance.GetComponentInChildren<MeshRenderer>()?.gameObject;
                if (mesh != null && mesh.GetComponent<Collider>() == null)
                {
                    mesh.AddComponent<BoxCollider>();
                }

                PrefabUtility.SaveAsPrefabAsset(instance, assetPath);
                GameObject.DestroyImmediate(instance);
            }
        }

    }
}

