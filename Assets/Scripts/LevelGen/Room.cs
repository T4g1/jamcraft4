using UnityEngine;
public class Room
{
    public GameObject instance;
    public Rect rect;
    public Rect room;
    public bool isStart, isEnd;
    public Room(GameObject mobject, Rect mrect, Rect mroom)
    {
        instance = mobject;
        rect = mrect;
        room = mroom;
        isStart = false;
        isEnd = false;
    }
    public void MarkStartEnd()
    {
        if (isStart) instance.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);
        if (isEnd) instance.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 1f);
    }

}