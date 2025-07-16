using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    [SerializeField] Transform respawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 위치 이동
            other.transform.position = respawn.position;

            // 데미지 주기 시도
            if (other.TryGetComponent<WalkerBase>(out var player))
            {
                player.TakeDamage(6); // 즉사 or 낙사 데미지 (원하는 수치로 조정 가능)
            }
        }
    }
}
