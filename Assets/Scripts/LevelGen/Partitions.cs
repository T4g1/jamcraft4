using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Partition // Each node of the Tree is considered as a Partition!
{
    public Rect room = new Rect(-1, -1, 0, 0); // null placeholder
    public Rect rect;
    public Partition left_child, right_child;
    public float weight;
    public Partition(Rect mrect)
    {
        rect = mrect;
    }

    public bool isLeaf()
    {
        return right_child == null && left_child == null;
    }

    public bool Split(int minRoomSize, int maxRoomSize, List<Vector2> allowed)
    {
        if (!isLeaf()) return false;
        float proportion = 1.2f;
        bool horizontal_split;
        if (rect.width / rect.height >= proportion)
        {
            horizontal_split = false;
        }
        else if (rect.height / rect.width >= proportion)
        {
            horizontal_split = true;

        }
        else
        {
            horizontal_split = UnityEngine.Random.Range(0.0f, 1.0f) > 0.3;
        }

        if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
        {
            return false;
        }

        if (horizontal_split)
        {
            int line = UnityEngine.Random.Range(maxRoomSize, (int)(rect.width - maxRoomSize));
            left_child = new Partition(new Rect(rect.x, rect.y, rect.width, line));
            right_child = new Partition(
                new Rect(rect.x, rect.y + line, rect.width, rect.height - line));

        }
        else
        {
            int line = UnityEngine.Random.Range(maxRoomSize, (int)(rect.height - maxRoomSize));

            left_child = new Partition(new Rect(rect.x, rect.y, line, rect.height));
            right_child = new Partition(
                new Rect(rect.x + line, rect.y, rect.width - line, rect.height));
        }
        return true;
    }

    public void CreateRoom(List<Vector2> allowed)
    {
        if (left_child != null)
        {
            left_child.CreateRoom(allowed);
        }
        if (right_child != null)
        {
            right_child.CreateRoom(allowed);
        }
        if (isLeaf())
        {
            int roomWidth = (int)rect.width;
            int roomHeight = (int)rect.height;
            int roomX = (int)rect.width - roomWidth;
            int roomY = (int)rect.height - roomHeight;

            room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
        }
    }

    public Rect GetRoom()
    {
        if (isLeaf()) return room;
        if (left_child != null)
        {
            Rect lroom = left_child.GetRoom();
            if (lroom.x != -1)
            {
                return lroom;
            }
        }
        if (right_child != null)
        {
            Rect rroom = right_child.GetRoom();
            if (rroom.x != -1)
            {
                return rroom;
            }
        }

        return new Rect(-1, -1, 0, 0); //nullplaceholder...!
    }

}