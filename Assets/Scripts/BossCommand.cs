using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

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


    protected float moveSpeed = 2f;
    protected int maxHp = 100;
    protected int currentHp;


    protected NavMeshAgent agent;
    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // 수동 회전 제어 (원하면 true)
        agent.updatePosition = true;
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
    public void MoveTo(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }
    protected void NextAction()
    {
        if (patternQueue.Count == 0)
        {
            SetupPattern(); // 패턴 반복
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
        // 여기에 선, 박스, 화살표, 디버그 도형 등 표시
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
            if (!p.activeInHierarchy) continue;
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

// 걷기
public class WalkCommand : ICommand
{
    private BossCommand boss;
    public WalkCommand(BossCommand b)
    {
        boss = b;
    }
    public void Execute()
    {
        boss.PlayAnim("Walk", 1f);
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
        boss.PlayAnim("Run", 2f); // 애니메이션
        if (boss.target != null)
            boss.MoveTo(boss.target.position);
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
    private float dashDistance = 5f;
    private float dashSpeed = 8f;

    public DashSkillCommand(BossCommand b)
    {
        boss = b;
        dashDir = b.GetTargetDirection();
    }

    public void Execute()
    {
        boss.StartCoroutine(ExecuteSkill());
    }

    IEnumerator ExecuteSkill()
    {
        boss.PlayAnim("Scream", 1f, false);
        boss.ShowDashPreview(dashDir, dashDistance);
        yield return new WaitForSeconds(1f);

        boss.PlayAnim("Run", 0.5f, false);
        float moved = 0f;
        while (moved < dashDistance)
        {
            float step = dashSpeed * Time.deltaTime;
            boss.transform.position += dashDir * step;
            moved += step;
            yield return null;
        }

        boss.PlayAnim("HornAttack", 1f, false);
        yield return new WaitForSeconds(1f);

        boss.OnActionComplete(); // 오직 여기서만 완료 처리
    }
}


