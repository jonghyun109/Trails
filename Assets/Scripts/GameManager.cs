using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] GameObject player1UI;
    [SerializeField] GameObject player2UI;
    GameObject boss;

    [SerializeField] private Slider bossHpSlider;
    [SerializeField] Transform bossSpawnPos;
    private void Awake()
    {
        SpawnBossIfMaster();
    }
    void Start()
    {
        boss = GameObject.FindWithTag("Boss");
        SpawnPlayer();
    }
    private void Update()
    {
        if (boss != null && boss.TryGetComponent<BossCommand>(out var bossScript))
        {
            bossScript.SetHpSlider(bossHpSlider);
        }
    }
    void SpawnBossIfMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var boss = PhotonNetwork.Instantiate("BossMob", bossSpawnPos.position, Quaternion.identity);

            if (boss.TryGetComponent<PhotonView>(out var pv) && pv.IsMine)
            {
                if (bossHpSlider != null && boss.TryGetComponent<BossCommand>(out var bossScript))
                {
                    pv.RPC("RPC_SetHpSlider", RpcTarget.AllBuffered);
                }
            }
        }
    }
    void SpawnPlayer()
    {
        int actorNum = PhotonNetwork.LocalPlayer.ActorNumber;

        string prefabName = actorNum == 1 ? "Player1" : "Player2";
        Vector3 spawnPos = actorNum == 1 ? new Vector3(-2, 0, 0) : new Vector3(2, 0, 0);

        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPos, Quaternion.identity);

        if (player.GetComponent<PhotonView>().IsMine)
        {
            // ī�޶� ����
            if (cam != null)
            {
                cam.Follow = player.transform;
                cam.LookAt = player.transform;
            }

            // UI Ȱ��ȭ
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
