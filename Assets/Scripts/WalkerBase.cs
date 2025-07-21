using Photon.Pun;
using UnityEngine;
using System.Collections;

public abstract class WalkerBase : MonoBehaviourPun, IWalker
{
    public int HP { get; private set; } = 6;
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

        UpdateHpUI();
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

        UpdateHpUI();

        if (PhotonNetwork.IsMasterClient)
        {
            RespawnManager.Instance.CheckRespawn();
        }

        yield return null;
    }

    public bool IsDead() => isDead;

    public void ForceRespawn()
    {
        if (!photonView.IsMine) return;

        StopAllCoroutines();
        StartCoroutine(Respawn_Internal());
    }

    private IEnumerator Respawn_Internal()
    {
        HP = 6;
        transform.position = spawnPosition;
        isDead = false;
        CanWalk = true;

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = true;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        var anim = GetComponentInChildren<Animator>();
        if (anim != null)
            anim.enabled = true;

        gameObject.SetActive(true);

        yield return Invincibility(1f);

        UpdateHpUI();
    }

    private void UpdateHpUI()
    {
        photonView.RPC("RPC_UpdateHpUI", RpcTarget.All, photonView.Owner.ActorNumber, HP);
    }

    [PunRPC]
    public void RPC_UpdateHpUI(int actorNumber, int hp)
    {
        foreach (var ui in FindObjectsOfType<HPUI>())
        {
            if (ui.targetActorNumber == actorNumber)
                ui.TryUpdateHp(actorNumber, hp);
        }
    }

    protected abstract void Move(Vector3 movement);
}
