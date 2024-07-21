using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Connecting to Photon...");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("Connected to Master");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning("Join Random Failed, Creating Room");
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 10 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        if (playerPrefab != null)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("Player Prefab is not assigned!");
        }
    }
}
