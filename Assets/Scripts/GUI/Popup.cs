using UnityEngine;

public class Popup : MonoBehaviour
{
    public event System.Action OnClose;
    public event System.Action OnOpen;
    [SerializeField]
    private bool openByDefault = false;


    protected virtual void Start()
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

    public virtual void Open()
    {
        gameObject.SetActive(true);

        if (OnOpen != null) {
            OnOpen();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (OnClose != null) {
            OnClose();
        }
    }
}