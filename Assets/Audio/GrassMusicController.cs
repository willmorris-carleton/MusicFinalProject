using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassMusicController : MusicAreaController
{
    [SerializeField]
    Light sceneDirectionalLight;
    [SerializeField]
    Light playerFlashLight;
    
    [SerializeField]
    float lightFadeOutTime = 1.0f;
    
    IEnumerator coroutine = null;

    void Start() {
        playerFlashLight.intensity = 0;
    }

    public override void OnTriggerEnter2D(Collider2D col) {
        base.OnTriggerEnter2D(col);
        FadeIn();
    }
    public override void OnTriggerExit2D(Collider2D col) {
        base.OnTriggerExit2D(col);
        FadeOut();
    }
    
    void FadeIn() {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = FadeBetweenLights(sceneDirectionalLight, playerFlashLight);
        StartCoroutine(coroutine);
    }

    void FadeOut() {
        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = FadeBetweenLights(playerFlashLight, sceneDirectionalLight);
        StartCoroutine(coroutine);
    }

    IEnumerator FadeBetweenLights(Light fromLight, Light toLight) {
        float fromStartIntensity = fromLight.intensity;
        float toStartIntensity = toLight.intensity;
        float startTime = Time.time;
        while (Time.time-startTime < lightFadeOutTime) {
            float t = (Time.time-startTime)/lightFadeOutTime;
            fromLight.intensity = Mathf.Lerp(fromStartIntensity, 0, t);
            toLight.intensity = Mathf.Lerp(toStartIntensity, 1, t);
            yield return null;
        }
    }
}