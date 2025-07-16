using Photon.Pun;
using UnityEngine;
using System.Collections;

public abstract class WalkerBase : MonoBehaviourPun, IWalker
{
    public int HP { get; private set; } = 6;           // 3칸 = 6
    public float MoveSpeed { get; set; } = 10f;
    public bool CanWalk { get; set; } = true;
    public Vector3 Direction { get; set; } = Vector3.zero;

    protected bool isInvincible = false;
    protected bool isDead = false;
    protected Vector3 spawnPosition;

    private void Awake()
    {
        spawnPosition = transform.position;
    }

    public virtual void Walk(Vector3 dir)
    {
        if (CanWalk && !isDead)
            Move(dir.normalized * MoveSpeed);
    }

    public virtual void TakeDamage(int amount)
    {
        if (!photonView.IsMine || isInvincible || isDead) return;

        HP -= amount;
        if (HP <= 0)
        {
            HP = 0;
            StartCoroutine(HandleDeath());
        }
        else
        {
            StartCoroutine(Invincibility(1f));
        }

        photonView.RPC("RPC_BroadcastHp", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, HP);
    }

    protected IEnumerator Invincibility(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }

    protected virtual IEnumerator HandleDeath()
    {
        isDead = true;
        CanWalk = false;
        gameObject.SetActive(false);

        // 체력 0 전송
        photonView.RPC("RPC_BroadcastHp", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber, 0);

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(RespawnCheckCoroutine());

        yield return null;
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
                // 전원 사망: 게임오버 등 처리 가능
                Debug.Log("All players dead.");
                yield break;
            }
        }

        photonView.RPC("RPC_Respawn", RpcTarget.All);
    }

    protected bool AnyAlivePlayer()
    {
        foreach (var p in FindObjectsOfType<WalkerBase>())
        {
            if (!p.isDead) return true;
        }
        return false;
    }

    [PunRPC]
    public void RPC_Respawn()
    {
        HP = 6;
        transform.position = spawnPosition;
        gameObject.SetActive(true);
        isDead = false;
        CanWalk = true;

        StartCoroutine(Invincibility(1f));
        photonView.RPC("RPC_BroadcastHp", RpcTarget.All, photonView.Owner.ActorNumber, HP);
    }

    [PunRPC]
    public void RPC_BroadcastHp(int actorNumber, int hp)
    {
        HP = hp;

        foreach (var ui in FindObjectsOfType<HPUI>())
        {
            ui.TryUpdateHp(actorNumber, hp);
        }
    }

    protected abstract void Move(Vector3 movement);
}
