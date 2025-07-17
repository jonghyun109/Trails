using UnityEngine;
using Photon.Pun;

public class TeleportWall : MonoBehaviour
{
    [SerializeField] Transform respawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = respawn.position;

            if (other.TryGetComponent<WalkerBase>(out var player))
            {
                player.TakeDamage(6); // 즉사
            }

            // 마스터 클라이언트면 리스폰 체크 시작
            if (PhotonNetwork.IsMasterClient)
            {
                RespawnManager.Instance.CheckRespawn();
            }
        }
    }
}