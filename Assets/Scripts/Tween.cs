using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public event System.Action OnTweenEnd;

    [SerializeField]
    private float transitionTime = 0.0f;

    private float from;
    private float to;
    private float elapsed_time;


    public void Interpolate(float from, float to)
    {
        this.from = from;
        this.to = to;

        elapsed_time = 0.0f;
    }

    void FixedUpdate()
    {
        if (elapsed_time < transitionTime) {
            elapsed_time += Time.fixedDeltaTime;
        }

        if (elapsed_time > transitionTime) {
            elapsed_time = transitionTime;
            
            if (OnTweenEnd != null) {
                OnTweenEnd();
            }
        }
    }

    public float GetValue()
    {
        if (transitionTime > 0.0f) {
            return Mathf.Lerp(from, to, elapsed_time / transitionTime);
        }

        return to;
    }
}