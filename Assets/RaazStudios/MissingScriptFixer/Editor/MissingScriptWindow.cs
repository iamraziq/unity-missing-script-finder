using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MissingScriptFixer
{
    public class MissingScriptWindow : EditorWindow
    {
        private Vector2 scrollPos;
        private List<MissingScriptRecord> records = new List<MissingScriptRecord>();
        private HashSet<int> selected = new HashSet<int>();

        [MenuItem("Tools/Missing Script Fixer")]
        public static void OpenWindow()
        {
            GetWindow<MissingScriptWindow>("Missing Script Fixer");
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Scan Project", GUILayout.Height(30)))
            {
                ScanProject();
            }

            GUILayout.Space(10);

            if (records.Count == 0)
            {
                EditorGUILayout.HelpBox("No missing scripts found.", MessageType.Info);
                return;
            }

            if (GUILayout.Button("Fix All Missing Scripts", GUILayout.Height(25)))
            {
                MissingScriptFixer.Fix(records);
                ScanProject();
                return;
            }

            GUILayout.Space(5);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            for (int i = 0; i < records.Count; i++)
            {
                var rec = records[i];

                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField("Asset:", rec.assetPath);
                EditorGUILayout.LabelField("Object:", rec.gameObjectName);
                EditorGUILayout.LabelField("Component Index:", rec.componentIndex.ToString());

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Ping"))
                {
                    EditorGUIUtility.PingObject(rec.gameObjectRef);
                }

                if (GUILayout.Button("Select"))
                {
                    Selection.activeGameObject = rec.gameObjectRef;
                }

                bool isSel = selected.Contains(i);
                bool newSel = GUILayout.Toggle(isSel, "Select This");

                if (newSel && !isSel) selected.Add(i);
                if (!newSel && isSel) selected.Remove(i);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndScrollView();

            GUILayout.Space(10);

            // Fix selected only
            if (selected.Count > 0)
            {
                if (GUILayout.Button("Fix Selected", GUILayout.Height(25)))
                {
                    List<MissingScriptRecord> sel = new List<MissingScriptRecord>();
                    foreach (int index in selected)
                        sel.Add(records[index]);

                    MissingScriptFixer.Fix(sel);
                    ScanProject();
                }
            }
        }

        private void ScanProject()
        {
            records = MissingScriptFinder.ScanProject();
            selected.Clear();
        }
    }
}
