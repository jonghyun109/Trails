using Photon.Pun;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] GameObject player1UI;
    [SerializeField] GameObject player2UI;

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

        if (player.GetComponent<PhotonView>().IsMine)
        {
            // 카메라 설정
            if (cam != null)
            {
                cam.Follow = player.transform;
                cam.LookAt = player.transform;
            }

            // UI 활성화
            if (actorNum == 1 && player1UI != null)
            {
                player1UI.SetActive(true);
            }
            else if (actorNum == 2 && player2UI != null)
            {
                player2UI.SetActive(true);
            }
        }
    }
}
