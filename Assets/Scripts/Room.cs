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

    public List<Room> connectedTo = new List<Room>();


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

    public GameObject Generate(Tilemap tilemap, GameObject dynamicHolder)
    {
        // Place tiles
        contentTilemap.CompressBounds();

        Vector3Int startCell = tilemap.WorldToCell(GetBottomLeft());

        BoundsInt bounds = contentTilemap.cellBounds;
        TileBase[] allTiles = contentTilemap.GetTilesBlock(bounds);

        List<TileBase> overrideTiles = new List<TileBase>();
        overrideTiles.Add(null);
        overrideTiles.Add(wall);

        for (int x = -1; x < bounds.size.x + 1; x++) {
            for (int y = -1; y < bounds.size.y + 1; y++) {
                Vector3Int cellPosition = startCell + new Vector3Int(x, y, 0);
                
                if (x < 0 || 
                    y < 0 || 
                    x >= bounds.size.x || 
                    y >= bounds.size.y
                ) {
                    TileBase currentTile = tilemap.GetTile(cellPosition);
                    if (overrideTiles.Contains(currentTile)) {
                        tilemap.SetTile(cellPosition, wall);
                    } 
                } else {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    tilemap.SetTile(cellPosition, tile);
                }
            }
        }

        // Place dynamic content
        GameObject roomContent = Instantiate(contentContainer);
        roomContent.transform.parent = dynamicHolder.transform;
        roomContent.transform.position = GetBottomLeft();
        
        return roomContent;
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

    public void DisableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
