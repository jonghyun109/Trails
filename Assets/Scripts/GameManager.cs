using Photon.Pun;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] CinemachineVirtualCamera cam;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;

        string prefabName = actorNum == 1 ? "Player1" : "Player2";
        Vector3 spawnPos = actorNum == 1 ? new Vector3(-2, 0, 0) : new Vector3(2, 0, 0);

        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);

        if (player.GetComponent<PhotonView>().IsMine && cam != null)
        {
            cam.Follow = player.transform;
            cam.LookAt = player.transform;
        }
    }
}
