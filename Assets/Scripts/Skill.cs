using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float speed = 15f;
    public float explodeDelay = 3f;

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
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GameObject effect = ObjectPool.Instance.GetEffect();
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
        }

        if(gameObject!=null)
        gameObject.GetComponentInChildren<Renderer>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        if(effect!=null)
        effect.SetActive(false);

        Destroy(gameObject); 
    }
}
