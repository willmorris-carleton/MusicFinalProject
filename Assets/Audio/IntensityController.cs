using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityController : MonoBehaviour
{
    public void SetIntensity(float intensity) {
        MusicManager.GetSongInstance().setParameterByName("Intensity", intensity);
    }
}
