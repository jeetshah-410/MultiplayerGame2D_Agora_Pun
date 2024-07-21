using JetBrains.Annotations;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerID;

    void Start()
    {
        playerID = GetUniquePlayerID();
        Debug.Log("Player ID : " +  playerID);
    }

    private int GetUniquePlayerID()
    {
        return Random.Range(1, 1000000);
    }
}
