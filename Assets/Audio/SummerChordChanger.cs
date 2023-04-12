using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerChordChanger : MonoBehaviour
{

    [SerializeReference]
    GrassMusicController grassMusicController;
    
    [SerializeField, Range(0,4)]
    int noteId = 0;

    SpriteRenderer sprite;

    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        MusicManager.GetSongInstance().setParameterByName("Note", 5);
    }

    void OnMouseOver() {
        if (grassMusicController.isInArea && sprite.enabled) {
            MusicManager.GetSongInstance().setParameterByName("Note", noteId);
            Debug.Log("Test" + noteId);
        }
    }

    void Update() {
        sprite.enabled = MusicManager.Instance.isAllowedToPlayIcicle;
    }
}
