using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    [SerializeField] Transform respawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ��ġ �̵�
            other.transform.position = respawn.position;

            // ������ �ֱ� �õ�
            if (other.TryGetComponent<WalkerBase>(out var player))
            {
                player.TakeDamage(6); // ��� or ���� ������ (���ϴ� ��ġ�� ���� ����)
            }
        }
    }
}
