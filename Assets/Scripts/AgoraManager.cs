
using UnityEngine;
using Agora.Rtc;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;


public class AgoraManager : MonoBehaviour
{

    [SerializeField]
    private string appID = "74d5e50ab6fa4472bfa4adf14b400d0f";
    public string token = "007eJxTYJibWeYfzti6pV97xkuDDdeaPM+q+4tU+j4TvqdUwZAqfFaBwdwkxTTV1CAxySwt0cTE3CgJSCWmpBmaJJkYGKQYpC28PietIZCRwW6iACMjAwSC+MwMjgUFDAwAwXQdjg==";
    private IRtcEngine rtcEngine;
    public GameObject videoViewPrefab;
    public Transform videoContainer;
    
    readonly private Dictionary<uint, GameObject> videoSurfaces = new();


    void Start()
    {
        InitializeAgora();
    }

    private void InitializeAgora()
    {
        rtcEngine = RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new(){ 
            appId = appID, 
            channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
            audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT,
        };
          
        rtcEngine.Initialize(context);
        InitEventHandler();
        
    }

    public void Join(string channelName)
    {
        rtcEngine.EnableVideo();
        rtcEngine.EnableLocalAudio(true);
        rtcEngine.EnableLocalVideo(true);
        ChannelMediaOptions options = new();
        options.autoSubscribeAudio.SetValue(true);
        options.autoSubscribeVideo.SetValue(true);
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        rtcEngine.JoinChannel(token, channelName, 0, options);
    }
    

    public void LeaveChannel()
    {
        rtcEngine.DisableVideo();
        rtcEngine.EnableLocalAudio(false);
        rtcEngine.EnableLocalVideo(false);
        ClearAllVideoSurfaces();
        rtcEngine.LeaveChannel();
    }

    private void InitEventHandler()
    {
        UserEventHandler handler = new (this);
        rtcEngine.InitEventHandler(handler);
    }

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly AgoraManager Manager;
        internal UserEventHandler(AgoraManager manager)
        {
            Manager = manager;
        }
        public override void OnError(int err, string msg)
        {
            Debug.Log("Error in IRtcEngineEventHandler: " + msg);
        }
        public override void OnJoinChannelSuccess(RtcConnection connection,int elapsed)
        {
            Debug.Log("Joined channel successfully: " + connection.channelId);

        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            if (Manager.videoContainer == null)
            {
                Debug.LogError("Video grid is not set.");
                return;
            }
            
            if (Manager.videoViewPrefab == null)
            {
                 Debug.LogError("Video view prefab is not assigned.");
                 return;
            }
            GameObject videoSurface = Instantiate(Manager.videoViewPrefab, Manager.videoContainer);
            videoSurface.AddComponent<VideoSurface>().SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            videoSurface.AddComponent<VideoSurface>().SetEnable(true);

            Manager.videoSurfaces[uid] = videoSurface;

            Debug.Log("Remote user joined: " + uid);
        }

        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            if (Manager.videoSurfaces.ContainsKey(uid))
            {
                GameObject videoSurface = Manager.videoSurfaces[uid];
                Destroy(videoSurface);
                Manager.videoSurfaces.Remove(uid);
            }
        }
    }

    private void ClearAllVideoSurfaces()
    {
        foreach (var surface in videoSurfaces.Values)
        {
            Destroy(surface);
        }
        videoSurfaces.Clear();
    }

    void OnDestroy()
    {
        if (rtcEngine != null)
        {
            rtcEngine.Dispose();
            rtcEngine = null;
        }
    }
}
