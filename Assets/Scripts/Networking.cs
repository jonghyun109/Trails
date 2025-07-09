using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
public class Networking : MonoBehaviourPunCallbacks
{
    [SerializeField]  CinemachineVirtualCamera cam;
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

        Vector3 spawnPos = new Vector3(-2 + actorNum * 4, 0);
        GameObject p = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);

        if (p.GetComponent<PhotonView>().IsMine)
        {
            cam.Follow = p.transform;
            cam.LookAt = p.transform;
        }
    }

}
