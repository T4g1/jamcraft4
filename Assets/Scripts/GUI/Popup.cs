using UnityEngine;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private bool openByDefault = false;


    void Start()
    {
        if (openByDefault) {
            Open();
        }
        else {
            Close();
        }
    }

    public bool IsOpen()
    {
        return gameObject.activeSelf;
    }

    public void Toggle()
    {
        if (gameObject.activeSelf) {
            Close();
        } else {
            Open();
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}