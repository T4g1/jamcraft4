using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    [SerializeField]
    private GameObject credits = null;
    [SerializeField]
    private GameObject creditsEndPosition = null;

    [SerializeField]
    private Tween creditsPositionTween = null;

    [SerializeField]
    private float freezeTime = 2.0f;


    void Start()
    {
        Assert.IsNotNull(credits);
        Assert.IsNotNull(creditsEndPosition);
        Assert.IsNotNull(creditsPositionTween);

        creditsPositionTween.OnTweenEnd += OnCreditsEnd;
        
        creditsPositionTween.Interpolate(
            credits.transform.localPosition.y, 
            creditsEndPosition.transform.localPosition.y
        );
    }

    void Update()
    {
        credits.transform.localPosition = new Vector2(
            credits.transform.localPosition.x,
            creditsPositionTween.GetValue()
        );
    }

    public void OnCreditsEnd()
    {
        StartCoroutine(_OnCreditsEnd());
    }

    void OnDestroy() 
    {
        creditsPositionTween.OnTweenEnd -= OnCreditsEnd;
    }

    IEnumerator _OnCreditsEnd()
    {
        yield return new WaitForSeconds(freezeTime);
        
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
