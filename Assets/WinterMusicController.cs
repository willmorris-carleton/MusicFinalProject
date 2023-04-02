using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WinterMusicController : MonoBehaviour
{
    BoxCollider2D boxCollider2D;
    EventInstance song;
    void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        song = AudioManager.Instance.CreateEventInstance(AudioManager.Instance.winterSong);
    }

    void OnTriggerEnter2D(Collider2D col) {
        song.start();
    }

    void OnTriggerExit2D(Collider2D col) {
        song.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
