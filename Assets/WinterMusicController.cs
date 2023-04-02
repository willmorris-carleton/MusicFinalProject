using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WinterMusicController : MonoBehaviour
{
    [SerializeField]
    LayerMask icicleLayer;
    
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

    void Update() {

        if (Input.GetMouseButton(0)) {
            song.setParameterByName("PlayingNote", 1);
        }
        else {
            song.setParameterByName("PlayingNote", 0);
        }
        
        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 0.1f, icicleLayer);
        if (hit.collider) {
            IcicleController icicle = hit.collider.GetComponent<IcicleController>();
            if (icicle) {
                Debug.Log(icicle.noteIndex);
                song.setParameterByName("Note", icicle.noteIndex);
                return;
            }
        }
        song.setParameterByName("Note", 0);
    }
}
