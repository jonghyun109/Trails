using UnityEditor;
using UnityEngine;

public class Boss_Toad : BossCommand
{
    protected override void Awake()
    {
        moveSpeed = 3.5f;
        maxHp = 200;
        base.Awake();
    }
    protected override void SetupPattern()
    {
        patternQueue.Enqueue(new WalkCommand(this));
        patternQueue.Enqueue(new ChaseCommand(this));
        patternQueue.Enqueue(new DashSkillCommand(this));
        //patternQueue.Enqueue(new WalkCommand(this));
        //patternQueue.Enqueue(new ChaseCommand(this));
        //patternQueue.Enqueue(new JumpAttackCommand(this));
    }

    protected override void Update()
    {
        base.Update();

        if (isExecuting && target != null)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            dir.y = 0f; // ���� ȸ���� �ϰԲ� Y ����

            // �̵�
            transform.position += dir * moveSpeed * Time.deltaTime;

            // ȸ��
            if (dir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }

}