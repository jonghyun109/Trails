using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public interface ICommand
{
    void Execute();
}

public abstract class BossCommand : MonoBehaviourPun
{
    protected Animator anim;
    public Transform target;
    protected Queue<ICommand> patternQueue = new Queue<ICommand>();
    protected bool isExecuting = false;
    public bool canMove = true;

    protected float moveSpeed = 2f;
    [SerializeField]protected int maxHp = 100;
    protected int currentHp;

    protected virtual void Awake()
    {
       
        anim = GetComponent<Animator>();
        currentHp = maxHp;
    }

    protected virtual void Start()
    {
        SetupPattern();
        NextAction();
    }

    protected virtual void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine) return;
    }
    
    protected void NextAction()
    {
        if (patternQueue.Count == 0)
        {
            SetupPattern();
        }

        ICommand next = patternQueue.Dequeue();
        isExecuting = true;
        next.Execute();
    }

    public void OnActionComplete()
    {
        isExecuting = false;
        StartCoroutine(HandlePostAction());
    }

    protected abstract void SetupPattern();

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        isExecuting = true;
        anim.Play("Die");
        StopAllCoroutines();

        Destroy(gameObject, 3f);
    }

    public void PlayAnim(string name, float delay = 0f, bool autoComplete = true)
    {
        StartCoroutine(PlayAndWait(name, delay, autoComplete));
    }

    IEnumerator PlayAndWait(string name, float delay, bool autoComplete)
    {
        anim.Play(name);
        yield return new WaitForSeconds(delay);
        if (autoComplete)
        {
            OnActionComplete();
        }
    }

    public void ShowDashPreview(Vector3 direction, float range)
    {
        //사거리 표시 
        Debug.DrawRay(transform.position, direction * range, Color.red, 1f);
    }
    public Vector3 GetTargetDirection()
    {
        if (target == null) return transform.forward;
        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0; // 수평 방향만 유지
        return dir;
    }
    

    private IEnumerator HandlePostAction()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateTarget();                       
        NextAction();                       
    }
    protected void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = p;
            }
        }

        if (closest != null)
            SetTarget(closest.transform);
    }
    public void OnDamagedBy(GameObject attacker)
    {
        SetTarget(attacker.transform);
    }
}

//Idle
public class IdleCommand : ICommand
{
    private BossCommand boss;
    public IdleCommand(BossCommand b)
    {
        boss = b;
    }
    public void Execute()
    {
        boss.canMove = false;
        boss.PlayAnim("Idle", 1f);
    }
}

// 추격
public class ChaseCommand : ICommand
{
    private BossCommand boss;

    public ChaseCommand(BossCommand b)
    {
        boss = b;
    }

    public void Execute()
    {
        boss.canMove = true;
        boss.PlayAnim("Walk", 2f);
    }
}

// 점프 공격
public class JumpAttackCommand : ICommand
{
    private BossCommand boss;
    public JumpAttackCommand(BossCommand b)
    {
        boss = b;
    }
    public void Execute()
    {
        boss.PlayAnim("JumpAttack", 2f);
    }
}

//스킬1 돌진
public class DashSkillCommand : ICommand
{
    private BossCommand boss;
    private Vector3 dashDir;
    private float dashDistance = 10f;
    private float dashSpeed = 10f;
    


    public DashSkillCommand(BossCommand b)
    {
        boss = b;
    }

    public void Execute()
    {
        boss.StartCoroutine(RushSkill());
    }

    IEnumerator RushSkill()
    {
        boss.canMove = false;
        boss.PlayAnim("Scream", 1.5f, false);

        yield return new WaitForSeconds(1f);

        dashDir = boss.GetTargetDirection();
        boss.ShowDashPreview(dashDir, dashDistance);

        yield return new WaitForSeconds(1.667f);

        boss.canMove = true;

        float moved = 0f;
        while (moved < dashDistance)
        {
            float step = dashSpeed * Time.deltaTime;
            boss.transform.position += dashDir * step;

            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, Quaternion.LookRotation(dashDir), 10f * Time.deltaTime);
            
            moved += step;
            yield return null;
        }


        yield return new WaitForSeconds(1f);

        boss.canMove = false;
        boss.OnActionComplete();
    }

}


