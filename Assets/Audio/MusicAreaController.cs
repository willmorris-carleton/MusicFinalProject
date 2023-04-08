using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAreaController : MonoBehaviour
{
    LayerMask MusicAreaLayer;
    
    [SerializeField]
    MusicArea musicAreaType;
    
    [SerializeField]
    ParticleController particles;

    void Awake() {
        MusicAreaLayer = LayerMask.NameToLayer("CharacterTrigger");
    }

    public virtual void OnTriggerEnter2D(Collider2D col) {
        particles.FadeIn();
    }

    public void OnTriggerStay2D(Collider2D other) {
        MusicManager.ChangeToMusicArea(musicAreaType);
    }

    public virtual void OnTriggerExit2D(Collider2D col) {
        MusicManager.ChangeToMusicArea(MusicArea.None);
        particles.FadeOut();
    }

}
