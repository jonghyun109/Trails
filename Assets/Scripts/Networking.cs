using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Networking : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;
        string prefabName = actorNum == 1 ? "Player1" : "Player2";

        Vector2 spawnPos = new Vector2(-2 + actorNum * 4, 0); // 간단한 위치 분리
        PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);
    }
}
