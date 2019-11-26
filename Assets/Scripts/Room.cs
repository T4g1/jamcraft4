using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField]
    private TileBase wall = null;

    private GameObject roomContent = null;
    private Tilemap contentTilemap;
    private GameObject contentContainer;

    void Start()
    {
        Assert.IsNotNull(wall);
    }

    public void SetContent(GameObject content)
    {
        roomContent = content;

        transform.localScale = roomContent.transform.Find("Size").localScale;

        Transform tilemapTransform = roomContent.transform.Find("Grid/Tilemap");
        contentTilemap = tilemapTransform.gameObject.GetComponent<Tilemap>();


        contentContainer = content.transform.Find("Content").gameObject;
    }

    public void Generate(Tilemap tilemap, GameObject dynamicHolder)
    {
        contentTilemap.CompressBounds();

        Vector3Int startCell = tilemap.WorldToCell(GetBottomLeft());

        BoundsInt bounds = contentTilemap.cellBounds;
        TileBase[] allTiles = contentTilemap.GetTilesBlock(bounds);

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

        GameObject roomContent = Instantiate(contentContainer);
        roomContent.transform.parent = dynamicHolder.transform;
        roomContent.transform.position = GetPosition();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Vector3Int GetBottomLeft()
    {
        return new Vector3Int(
            Mathf.FloorToInt(GetPosition().x - transform.localScale.x / 2),
            Mathf.FloorToInt(GetPosition().y - transform.localScale.y / 2),
            0
        );
    }
}
