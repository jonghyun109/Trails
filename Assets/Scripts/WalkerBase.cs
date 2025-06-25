using Photon.Pun;
using UnityEngine;

public abstract class WalkerBase : MonoBehaviourPun, IWalker
{
    public float MoveSpeed { get; set; } = 10f;
    public bool CanWalk { get; set; } = true;
    public Vector3 Direction { get; set; } = Vector3.zero;

    public void Walk(Vector3 dir)
    {
        Move(dir.normalized * MoveSpeed);
    }

    protected abstract void Move(Vector3 movement);
}
