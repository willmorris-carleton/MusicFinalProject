using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD.Studio;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class WinterMusicController : Singleton<WinterMusicController>
{
    [SerializeField]
    LayerMask icicleLayer;
    
    BoxCollider2D boxCollider2D;
    EventInstance song;
    

    bool isAllowedToPlayIcicle = false;
    
    #region FMOD_CALLBACKS
    // Variables that are modified in the callback need to be part of a seperate class.
    // This class needs to be 'blittable' otherwise it can't be pinned in memory.
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBar = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr) {
        // Retrieve the user data
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    timelineInfo.currentMusicBar = parameter.bar;
                }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    timelineInfo.lastMarker = parameter.name;
                    if (((string) timelineInfo.lastMarker).StartsWith("Playable"))
                    {
                        Instance.isAllowedToPlayIcicle = true;
                    }
                    else if (((string) timelineInfo.lastMarker).StartsWith("NotPlayable")) {
                        Instance.isAllowedToPlayIcicle = false;
                    }
                }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }
    #endregion
    
    TimelineInfo timelineInfo;
    GCHandle timelineHandle;
    
    void Start() {
        boxCollider2D = GetComponent<BoxCollider2D>();
        
        timelineInfo = new TimelineInfo();
        song = AudioManager.Instance.CreateEventInstance(AudioManager.Instance.winterSong);
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        song.setUserData(GCHandle.ToIntPtr(timelineHandle));
        song.setCallback(new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback), FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
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

    public static bool isPlayable() {
        return Instance.isAllowedToPlayIcicle;
    }
    
    void OnGUI()
    {
        GUILayout.Box(String.Format("Current Bar = {0}, Last Marker = {1}", timelineInfo.currentMusicBar, (string)timelineInfo.lastMarker));
    }
}
