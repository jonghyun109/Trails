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
                player.TakeDamage(6); // ���
            }

            // ������ Ŭ���̾�Ʈ�� ������ üũ ����
            if (PhotonNetwork.IsMasterClient)
            {
                RespawnManager.Instance.CheckRespawn();
            }
        }
    }
}