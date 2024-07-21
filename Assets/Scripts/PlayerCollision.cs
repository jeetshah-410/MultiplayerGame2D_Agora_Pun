using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollision : MonoBehaviourPun
{
    private AgoraManager agoraManager;
    private string channelName = "App";
    void Start()
    {
        agoraManager = GameObject.FindObjectOfType<AgoraManager>();
    }

    void OnTriggerEnter2D(Collider2D otherActor)
    {
        
        Debug.Log("Trigger Enter Detected");
        if (otherActor.CompareTag("Player"))
        {
            agoraManager.Join(channelName);
        }
    }

    void OnTriggerExit2D(Collider2D otherActor)
    {
        Debug.Log("Trigger Exit Detected");
        if (otherActor.CompareTag("Player"))
        {
                agoraManager.LeaveChannel();
        }
    }  
}
