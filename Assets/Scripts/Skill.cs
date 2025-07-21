using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    public float speed = 15f;
    public float explodeDelay = 3f;
    private bool alreadyExploded = false;

    private int damageAmount = 5;
    Boss_Toad boss;

    private void Start()
    {
        boss = FindObjectOfType<Boss_Toad>();
    }
    [PunRPC]
    public void SkillBoom(Vector3 targetPos)
    {
        StartCoroutine(MoveToTargetAndExplode(targetPos));
    }

    IEnumerator MoveToTargetAndExplode(Vector3 targetPos)
    {
        while ((targetPos - transform.position).magnitude >= 0.05f)
        {
            Vector3 dir = (targetPos - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(explodeDelay);

        if (!alreadyExploded)
        {
            alreadyExploded = true;
            StartCoroutine(Explode(0));
        }
    }

    [PunRPC]
    public void TriggerExplosion(bool isLightning)
    {
        if (alreadyExploded) return;
        alreadyExploded = true;

        if (isLightning)
        {
            explodeDelay = 0f; // ��� ����
            
        }
        damageAmount = 10;
        StartCoroutine(Explode(3));
        Debug.Log("ũ�� ������");
    }

    IEnumerator Explode(int effectId)
    {
        Debug.Log("����");
        GameObject effect = ObjectPool.Instance.GetEffect(effectId);
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, 2f, LayerMask.GetMask("Boss"));
        foreach (var hit in hits)
        {
            Debug.Log($"��ų ���� ��� : {hit.name}");
            if (hit.TryGetComponent<IBossDamageable>(out var boss))
            {
                Debug.Log("��ų �¾Ҵµ� ������ ����?");
                boss.BossTakeDamage(damageAmount);
            }
        }
        GetComponentInChildren<Renderer>().enabled = false;
        yield return new WaitForSeconds(0.5f);

        effect.SetActive(false);
        
        Destroy(gameObject);
    }
}
