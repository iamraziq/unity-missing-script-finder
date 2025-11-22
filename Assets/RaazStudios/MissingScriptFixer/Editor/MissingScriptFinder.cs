using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace MissingScriptFixer
{
    public static class MissingScriptFinder
    {
        public static List<MissingScriptRecord> ScanProject()
        {
            var results = new List<MissingScriptRecord>();

            // 1. Scan ALL scenes in build settings
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

                if (sceneAsset != null)
                    ScanScene(scene.path, results);
            }

            // 2. Scan ALL prefabs in project
            string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
            foreach (string guid in prefabGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null)
                    ScanGameObject(prefab, path, results);
            }

            return results;
        }

        private static void ScanScene(string scenePath, List<MissingScriptRecord> results)
        {
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            foreach (GameObject root in scene.GetRootGameObjects())
            {
                ScanGameObject(root, scenePath, results);
            }
        }

        private static void ScanGameObject(GameObject root, string path, List<MissingScriptRecord> results)
        {
            Transform[] all = root.GetComponentsInChildren<Transform>(true);

            foreach (Transform t in all)
            {
                Component[] comps = t.gameObject.GetComponents<Component>();

                for (int i = 0; i < comps.Length; i++)
                {
                    if (comps[i] == null)
                    {
                        results.Add(new MissingScriptRecord(
                            path,
                            t.gameObject.name,
                            t.gameObject,
                            i
                        ));
                    }
                }
            }
        }
    }
}
