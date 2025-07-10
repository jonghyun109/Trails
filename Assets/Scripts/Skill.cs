using System.Collections;
using Photon.Pun;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    public float speed = 15f;
    public float explodeDelay = 3f;
    private bool alreadyExploded = false;

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
        StartCoroutine(Explode());
    }

    [PunRPC]
    public void TriggerExplosion(bool isLightning)
    {
        if (alreadyExploded) return;
        alreadyExploded = true;

        if (isLightning)
        {
            explodeDelay = 0f; // Áï½Ã Æø¹ß
        }

        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        GameObject effect = ObjectPool.Instance.GetEffect(0);
        if (effect != null)
        {
            effect.transform.position = transform.position;
            effect.transform.rotation = Quaternion.identity;
            effect.SetActive(true);
        }

        if (gameObject != null)
            GetComponentInChildren<Renderer>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        if (effect != null)
            effect.SetActive(false);

        Destroy(gameObject);
    }
}
