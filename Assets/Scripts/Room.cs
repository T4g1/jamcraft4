using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    [SerializeField]
    public TileBase floor = null;
    
    [SerializeField]
    public TileBase wall = null;

    [SerializeField]
    private Vector2Int size = Vector2Int.zero;
    public Vector2Int Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
            transform.localScale = new Vector3(
                size.x,
                size.y,
                0.0f
            );
        }
    }

    void Start()
    {
        Assert.IsNotNull(floor);
        Assert.IsNotNull(wall);
    }

    public void Generate(Tilemap tilemap)
    {
        Vector3Int startCell = tilemap.WorldToCell(GetBottomLeft());
        for (int x=0; x<size.x; x++) {
            for (int y=0; y<size.y; y++) {
                tilemap.SetTile(startCell + new Vector3Int(x, y, 0), floor);
            }
        }
    }

    public Vector3Int GetBottomLeft()
    {
        return new Vector3Int(
            Mathf.FloorToInt(transform.position.x - transform.localScale.x / 2),
            Mathf.FloorToInt(transform.position.y - transform.localScale.y / 2),
            0
        );
    }
}
