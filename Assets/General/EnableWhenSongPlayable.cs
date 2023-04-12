using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableWhenSongPlayable : MonoBehaviour
{

    [SerializeField]
    List<GameObject> objectsToControl;

    // Update is called once per frame
    void Update() {
        foreach (GameObject obj in objectsToControl) {
            
            obj.SetActive(MusicManager.Instance.isAllowedToPlayIcicle);
        }
    }
}
