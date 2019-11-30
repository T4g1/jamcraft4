using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "End Item", menuName = "Items/New End Item")]
public class EndItem : Item
{
    public override void OnPickedUp()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }
}