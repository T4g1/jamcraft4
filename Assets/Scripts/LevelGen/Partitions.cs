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
        public Partition(Rect mrect){
            rect = mrect;
        }

        public bool isLeaf(){
            return right_child == null && left_child == null;
        }

        public bool Split(int minRoomSize, int maxRoomSize){
            if (!isLeaf())return false;
            bool horizontal_split;
            if (rect.width / rect.height >= 1.25) // See if 1.25 is a right proportion or not.
            {
                horizontal_split = false;
            }
            else if (rect.height / rect.width >= 1.25) // Same here!
            {
                horizontal_split = true;
            }
            else
            {
                horizontal_split = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5;
            }

            if (Mathf.Min(rect.height, rect.width) / 2 < minRoomSize)
            {
                return false;
            }

            if (horizontal_split)
            {
                int line = UnityEngine.Random.Range(minRoomSize, (int)(rect.width - minRoomSize));
                left_child = new Partition(new Rect(rect.x, rect.y, rect.width, line));
                right_child = new Partition(
                    new Rect(rect.x, rect.y + line, rect.width, rect.height - line));
            }
            else
            {
                int line = UnityEngine.Random.Range(minRoomSize, (int)(rect.height - minRoomSize));

                left_child = new Partition(new Rect(rect.x, rect.y, line, rect.height));
                right_child = new Partition(
                    new Rect(rect.x + line, rect.y, rect.width - line, rect.height));
            }
            return true;
        }

        public void CreateRoom()
        {
            if (left_child != null)
            {
                left_child.CreateRoom();
            }
            if (right_child != null)
            {
                right_child.CreateRoom();
            }
            if (isLeaf())
            {
                // int roomWidth = (int)rect.width;
                // int roomHeight = (int)rect.height;
                // int roomX = (int)rect.width;
                // int roomY = (int)rect.height;       <--- Messy attempt
                int roomWidth = (int)UnityEngine.Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)UnityEngine.Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)UnityEngine.Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)UnityEngine.Random.Range(1, rect.height - roomHeight - 1);

                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
            }
        }

        public Rect GetRoom(){
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