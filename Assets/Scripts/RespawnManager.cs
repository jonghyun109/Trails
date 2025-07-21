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
                PhotonNetwork.LoadLevel(0); // Àü¸ê ½Ã
                yield break;
            }
        }

        foreach (var walker in FindObjectsOfType<WalkerBase>())
        {
            if (walker.IsDead())
            {
                photonView.RPC("RPC_TriggerLocalRespawn", walker.photonView.Owner);
            }
        }
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
    private void RPC_TriggerLocalRespawn()
    {
        foreach (var walker in FindObjectsOfType<WalkerBase>())
        {
            if (walker.photonView.IsMine)
            {
                walker.ForceRespawn();
            }
        }
    }
}

