using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile
{
    [SerializeField]
    private Sprite[] waterSprites;

    [SerializeField]
    private Sprite preview;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        return base.StartUp(position, tilemap, go);
    }
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                Vector3Int nPos = new Vector3Int(position.x + x, position.y + y, position.z);
                if(HasWater(tilemap, nPos))
                {
                    tilemap.RefreshTile(nPos);
                }
            }
        }
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        string composition = string.Empty;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    if (HasWater(tilemap, new Vector3Int(position.x + x, position.y + y, position.z))) composition += 'W';
                    else composition += 'E';
                }
            }
        }
        

        tileData.sprite = waterSprites[4];

        if ((composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W') ||
                (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W') ||
                (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E'))
        {
            tileData.sprite = waterSprites[4];
            int randomVal = Random.Range(0, 100);
            if (randomVal <= 10) tileData.sprite = waterSprites[9];
        }
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W') tileData.sprite = waterSprites[3];
        else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W') tileData.sprite = waterSprites[6];
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W') tileData.sprite = waterSprites[3];
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W') tileData.sprite = waterSprites[7];
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E') tileData.sprite = waterSprites[5];
        else if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E') tileData.sprite = waterSprites[2];
        else if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E') tileData.sprite = waterSprites[5];
        else if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W') tileData.sprite = waterSprites[1];
        else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E') tileData.sprite = waterSprites[8];
        else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W') tileData.sprite = waterSprites[0];
    }

    private bool HasWater(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateWaterTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Watertile", "New watertile", "asset", "Save watertile", "Asssets");
        if (path == "") return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }
#endif
}
