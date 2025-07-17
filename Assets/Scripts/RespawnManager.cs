using UnityEngine;
using Photon.Pun;
using System.Collections;

public class RespawnManager : MonoBehaviourPun
{
    public static RespawnManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void CheckRespawn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(RespawnCheckCoroutine());
        }
    }

    private IEnumerator RespawnCheckCoroutine()
    {
        float waitTime = 3f;

        while (waitTime > 0)
        {
            if (AnyAlivePlayer())
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }
            else
            {
                Debug.Log("All players dead. Game Over");
                yield break;
            }
        }

        photonView.RPC("RPC_RespawnAll", RpcTarget.All);
    }

    private bool AnyAlivePlayer()
    {
        foreach (var walker in FindObjectsOfType<WalkerBase>())
        {
            if (walker == null) continue;

            if (!walker.IsDead()) return true;
        }
        return false;
    }

    [PunRPC]
    private void RPC_RespawnAll()
    {
        foreach (var walker in FindObjectsOfType<WalkerBase>(true))
        {
            walker.ForceRespawn();
        }
    }
}
