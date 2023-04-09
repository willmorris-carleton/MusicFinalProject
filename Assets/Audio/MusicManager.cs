using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMOD.Studio;
using System;
using UnityEngine;

public enum MusicArea {
    None,
    Winter,
    Fire,
    Grass
}

public class MusicManager : Singleton<MusicManager>
{
    
    EventInstance song;
    [HideInInspector]
    public bool isAllowedToPlayIcicle = false;
    MusicArea currentArea = MusicArea.None;
    MusicArea lastArea = MusicArea.None;
    
    #region FMOD_CALLBACKS
    
    TimelineInfo timelineInfo;
    GCHandle timelineHandle;
    
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBar = 0;
        public int currentMusicBeat = 0;
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
                    
                    if (parameter.bar > timelineInfo.currentMusicBar) {
                        BarOccured();
                    }
                    timelineInfo.currentMusicBar = parameter.bar;
                    
                    if (parameter.beat > timelineInfo.currentMusicBeat) {
                        BeatOccured();
                    }
                    timelineInfo.currentMusicBeat = parameter.beat;
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

    static List<Action> functionsToCallWhenBeatOccurs = new List<Action>();
    public static void SubscribeFunctionToBeat(Action func) {
        functionsToCallWhenBeatOccurs.Add(func);
    }
    static void BeatOccured() {
        foreach (Action func in functionsToCallWhenBeatOccurs) {
            func();
        }
    }
    
    static List<Action> functionsToCallWhenBarOccurs = new List<Action>();
    
    public static void SubscribeFunctionToBar(Action func) {
        functionsToCallWhenBarOccurs.Add(func);
    }
    static void BarOccured() {
        foreach (Action func in functionsToCallWhenBarOccurs) {
            func();
        }
    }
    
    void Start() {
        timelineInfo = new TimelineInfo();
        song = AudioManager.Instance.CreateEventInstance(AudioManager.Instance.mainSong);
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        song.setUserData(GCHandle.ToIntPtr(timelineHandle));
        song.setCallback(BeatEventCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }

    public static void ChangeToMusicArea(MusicArea area) {
        
        if (area == GetCurrentMusicArea()) return;
        
        if (Instance) {
            Instance.lastArea = Instance.currentArea;

            //Update fmod area parameter only if not none
            if (area != MusicArea.None)
                Instance.song.setParameterByNameWithLabel("MusicArea", area.ToString());
            
            //If transitioning from something to nothing, flag fmod to stop current song
            if (area == MusicArea.None && Instance.currentArea != MusicArea.None) Instance.song.stop(STOP_MODE.ALLOWFADEOUT);
            
            //If transitioning from nothing to something, flag fmod to start current song
            if (area != MusicArea.None && Instance.currentArea == MusicArea.None) Instance.song.start();
            
            Instance.currentArea = area;
        }
    }

    public static EventInstance GetSongInstance() {
        return Instance.song;
    }

    public static MusicArea GetCurrentMusicArea() {
        if (Instance) {
            return Instance.currentArea;
        }
        return MusicArea.None;
    }
    
    void OnGUI()
    {
        GUILayout.Box(String.Format("Current Bar = {0}, Last Marker = {1}, Current Area = {2}", timelineInfo.currentMusicBar, (string)timelineInfo.lastMarker, currentArea.ToString()));
    }
    
}
