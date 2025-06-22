using Photon.Pun;
using UnityEngine;

public abstract class WalkerBase : MonoBehaviourPun, IWalker
{
    public float MoveSpeed { get; set; } = 10f;
    public bool CanWalk { get; set; } = true;
    public Vector2 Direction { get; set; } = Vector2.zero;

    public void Walk(Vector2 dir)
    {
        Move(dir.normalized * MoveSpeed * Time.deltaTime);
    }

    protected abstract void Move(Vector2 movement);
}
