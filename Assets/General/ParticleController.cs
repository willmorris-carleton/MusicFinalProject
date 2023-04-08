using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField]
    float fadeOutTime = 1.0f;
    
    ParticleSystem particles;
    Renderer renderer;
    IEnumerator coroutine = null;
    
    // Start is called before the first frame update
    void Start() {
        particles = GetComponent<ParticleSystem>();
        renderer = GetComponent<Renderer>();
    }

    public void FadeIn() {
        if (coroutine != null)
            StopCoroutine(coroutine);
        
        //Reset Opacity
        Color c = renderer.material.color;
        c.a = 1;
        renderer.material.color = c;

        particles.Clear();
        particles.Play();
    }

    public void FadeOut() {
        if (coroutine != null)
            StopCoroutine(coroutine);
        
        coroutine = FadeOutRoutine();
        StartCoroutine(coroutine);
    }

    IEnumerator FadeOutRoutine()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        float startAlpha = renderer.material.color.a;
        float startTime = Time.time;
        Color c = renderer.material.color;
        while (Time.time-startTime < fadeOutTime) {
            float t = (Time.time-startTime)/fadeOutTime;
            float a = Mathf.Lerp(startAlpha, 0, t);
            c.a = a;
            renderer.material.color = c;
            yield return null;
        }
    }
}
