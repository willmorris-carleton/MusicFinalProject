using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleController : MonoBehaviour
{
    [Range(1,5)]
    public int noteIndex = 1;

    SpriteRenderer spriteRenderer;
    Color color;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
    }

    void Update() {
        if (WinterMusicController.isPlayable()) {
            spriteRenderer.color = color;
        }
        else {
            spriteRenderer.color = Color.gray;
        }
    }
}
