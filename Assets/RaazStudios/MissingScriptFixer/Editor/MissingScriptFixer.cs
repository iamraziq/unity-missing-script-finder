using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MissingScriptFixer
{
    public static class MissingScriptFixer
    {
        public static void Fix(List<MissingScriptRecord> records)
        {
            foreach (var record in records)
            {
                if (record.gameObjectRef == null) continue;

                SerializedObject so = new SerializedObject(record.gameObjectRef);
                SerializedProperty prop = so.FindProperty("m_Component");

                prop.DeleteArrayElementAtIndex(record.componentIndex);

                so.ApplyModifiedProperties();

                EditorUtility.SetDirty(record.gameObjectRef);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
