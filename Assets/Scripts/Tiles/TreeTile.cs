using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeTile : Tile
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/TreeTile")]
    public static void CreateTreeTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Treetile", "New treetile", "asset", "Save treetile", "Asssets");
        if (path == "") return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TreeTile>(), path);
    }
#endif
}
