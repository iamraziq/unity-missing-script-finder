using UnityEngine;

namespace MissingScriptFixer
{
    [System.Serializable]
    public class MissingScriptRecord
    {
        public string assetPath;
        public string gameObjectName;
        public GameObject gameObjectRef;
        public int componentIndex;

        public MissingScriptRecord(string path, string goName, GameObject goRef, int index)
        {
            assetPath = path;
            gameObjectName = goName;
            gameObjectRef = goRef;
            componentIndex = index;
        }
    }
}
