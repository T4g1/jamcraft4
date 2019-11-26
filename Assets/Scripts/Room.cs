using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField]
    private TileBase floor = null;
    
    [SerializeField]
    private TileBase wall = null;

    [SerializeField]
    private GameObject body = null;

    [SerializeField]
    private Tilemap content = null;

    void Start()
    {
        Assert.IsNotNull(body);
        Assert.IsNotNull(floor);
        Assert.IsNotNull(wall);
        Assert.IsNotNull(content);
    }

    public void Generate(Tilemap tilemap)
    {
        content.CompressBounds();

        Vector3Int startCell = tilemap.WorldToCell(GetBottomLeft());

        BoundsInt bounds = content.cellBounds;
        TileBase[] allTiles = content.GetTilesBlock(bounds);

        for (int x = -1; x < bounds.size.x + 1; x++) {
            for (int y = -1; y < bounds.size.y + 1; y++) {
                Vector3Int cellPosition = startCell + new Vector3Int(x, y, 0);
                
                if (x < 0 || 
                    y < 0 || 
                    x >= bounds.size.x || 
                    y >= bounds.size.y
                ) {
                    if (tilemap.GetTile(cellPosition) == null) {
                        tilemap.SetTile(cellPosition, wall);
                    } 
                } else {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    tilemap.SetTile(cellPosition, tile);
                }
            }
        }
    }

    public Vector3 GetPosition()
    {
        return body.transform.position;
    }

    public Vector3Int GetBottomLeft()
    {
        Vector3 bodyPosition = body.transform.position;
        Vector3 bodyScale = body.transform.localScale;

        return new Vector3Int(
            Mathf.FloorToInt(bodyPosition.x - bodyScale.x / 2),
            Mathf.FloorToInt(bodyPosition.y - bodyScale.y / 2),
            0
        );
    }
}
