using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    private GameObject roomContent = null;
    private Tilemap floorsTilemap;
    private Tilemap wallsTilemap;
    private GameObject contentContainer;

    public List<Room> connectedTo = new List<Room>();
    

    public void SetContent(GameObject content)
    {
        roomContent = content;

        transform.localScale = roomContent.transform.Find("Size").localScale;

        Transform floorsTransform = roomContent.transform.Find("Grid/Floors");
        Transform wallsTransform = roomContent.transform.Find("Grid/Walls");

        floorsTilemap = floorsTransform.gameObject.GetComponent<Tilemap>();
        wallsTilemap = wallsTransform.gameObject.GetComponent<Tilemap>();

        contentContainer = content.transform.Find("Content").gameObject;
    }

    public void GenerateWalls(Tilemap walls)
    {
        _Generate(wallsTilemap, walls);
    }

    public void GenerateFloors(Tilemap floors)
    {
        _Generate(floorsTilemap, floors);
    }

    private void _Generate(Tilemap source, Tilemap destination)
    {
        // Place tiles
        source.CompressBounds();

        Vector3Int startCell = destination.WorldToCell(GetBottomLeft());

        BoundsInt bounds = source.cellBounds;
        TileBase[] allTiles = source.GetTilesBlock(bounds);

        List<TileBase> overrideTiles = new List<TileBase>();
        overrideTiles.Add(null);
        overrideTiles.Add(Utility.GetWall());

        for (int x = -1; x < bounds.size.x + 1; x++) {
            for (int y = -1; y < bounds.size.y + 1; y++) {
                Vector3Int cellPosition = startCell + new Vector3Int(x, y, 0);
                
                if (x < 0 || 
                    y < 0 || 
                    x >= bounds.size.x || 
                    y >= bounds.size.y
                ) {
                    TileBase currentTile = destination.GetTile(cellPosition);
                    if (overrideTiles.Contains(currentTile)) {
                        //destination.SetTile(cellPosition, Utility.GetWall());
                    } 
                } else {
                    TileBase tile = allTiles[x + y * bounds.size.x];
                    destination.SetTile(bounds.position + cellPosition, tile);
                }
            }
        }
    }

    public GameObject GenerateContent(GameObject dynamicHolder)
    {
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
