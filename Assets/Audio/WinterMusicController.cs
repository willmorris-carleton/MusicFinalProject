using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WinterMusicController : MusicAreaController
{
    [SerializeField]
    LayerMask icicleLayer;

    void Update() {

        if (Input.GetMouseButton(0)) {
            MusicManager.GetSongInstance().setParameterByName("PlayingNote", 1);
        }
        else {
            MusicManager.GetSongInstance().setParameterByName("PlayingNote", 0);
        }

        RaycastHit2D hit;
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 0.1f, icicleLayer);
        if (hit.collider) {
            IcicleController icicle = hit.collider.GetComponent<IcicleController>();
            if (icicle) {
                Debug.Log(icicle.noteIndex);
                MusicManager.GetSongInstance().setParameterByName("Note", icicle.noteIndex);
                return;
            }
        }
        MusicManager.GetSongInstance().setParameterByName("Note", 0);
    }

    public static bool isPlayable() {
        return MusicManager.Instance.isAllowedToPlayIcicle;
    }
}
