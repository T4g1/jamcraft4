using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsText = null;
    [SerializeField]
    private GameObject creditsEndPosition = null;

    [SerializeField]
    private Tween creditsPositionTween = null;


    void Start()
    {
        Assert.IsNotNull(creditsText);
        Assert.IsNotNull(creditsEndPosition);
        Assert.IsNotNull(creditsPositionTween);

        creditsPositionTween.OnTweenEnd += OnCreditsEnd;
        
        creditsPositionTween.Interpolate(
            creditsText.transform.localPosition.y, 
            creditsEndPosition.transform.localPosition.y
        );
    }

    void Update()
    {
        creditsText.transform.localPosition = new Vector2(
            creditsText.transform.localPosition.x,
            creditsPositionTween.GetValue()
        );
    }

    void UpdateCreditsPosition()
    {
        Canvas canvas = creditsText.GetComponentInParent<Canvas>();
    }

    public void OnCreditsEnd()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    void OnDestroy() 
    {
        creditsPositionTween.OnTweenEnd -= OnCreditsEnd;
    }
}
