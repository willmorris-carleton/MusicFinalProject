using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireColour : MonoBehaviour
{
    ParticleSystem.MainModule particleSettings;
    Light light;

    public Color[] colors;

    void Start() {
        particleSettings = GetComponent<ParticleSystem>().main;
        light = GetComponent<Light>();
        MusicManager.SubscribeFunctionToBar(UpdateOnBeat);
    }

    void UpdateOnBeat() {
        MusicManager.GetSongInstance().getParameterByName("Note", out float currentNoteFloat);
        int currentNote = (int) Mathf.Clamp(currentNoteFloat, 0, 3);

        particleSettings.startColor = new ParticleSystem.MinMaxGradient(colors[currentNote], colors[currentNote]);
        light.color = colors[currentNote]*0.2f;
    }
}
