using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAreaController : MonoBehaviour
{
    [SerializeField]
    MusicArea musicAreaType;
    
    [SerializeField]
    ParticleController particles;

    public virtual void OnTriggerEnter2D(Collider2D col) {
        MusicManager.ChangeToMusicArea(musicAreaType);
        particles.FadeIn();
    }

    public virtual void OnTriggerExit2D(Collider2D col) {
        if (MusicManager.GetCurrentMusicArea() == musicAreaType) {
            MusicManager.ChangeToMusicArea(MusicArea.None);
        }
        particles.FadeOut();
    }
}
