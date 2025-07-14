using Photon.Pun;
using UnityEngine;

public abstract class WalkerBase : MonoBehaviourPun, IWalker
{
    public int HP { get; set; } = 10;
    public float MoveSpeed { get; set; } = 10f;
    public bool CanWalk { get; set; } = true;
    public Vector3 Direction { get; set; } = Vector3.zero;

    public virtual void Walk(Vector3 dir)
    {
        Move(dir.normalized * MoveSpeed);
    }
    public virtual void TakeDamage(int amount)
    {
        if (photonView.IsMine == false) return;

        HP -= amount;
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} Á×À½");
    }
    protected abstract void Move(Vector3 movement);
}
