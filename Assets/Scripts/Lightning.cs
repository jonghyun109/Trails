using UnityEngine;
using System.Collections;
using Photon.Pun;

public class Lightning : MonoBehaviourPun
{
    [PunRPC]
    public void StrikeLightning(Vector3 targetPos)
    {
        StartCoroutine(StrikeRoutine(targetPos));
    }

    IEnumerator StrikeRoutine(Vector3 targetPos)
    {
        GameObject warning = ObjectPool.Instance.GetEffect(1);
        if (warning != null)
        {
            warning.transform.position = targetPos;
            warning.transform.rotation = Quaternion.identity;
            warning.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        GameObject strike = ObjectPool.Instance.GetEffect(2);
        if (strike != null)
        {
            strike.transform.position = targetPos;
            strike.transform.rotation = Quaternion.identity;
            strike.SetActive(true);
        }

        // ÆøÅº Å½»ö ¹× RPC È£Ãâ
        Collider[] hits = Physics.OverlapSphere(targetPos, 1.5f);
        foreach (Collider col in hits)
        {
            Skill bomb = col.GetComponent<Skill>();
            if (bomb != null)
            {
                bomb.photonView.RPC("TriggerExplosion", RpcTarget.All, true);
            }
        }

        yield return new WaitForSeconds(0.5f);

        if (warning != null) warning.SetActive(false);
        if (strike != null) strike.SetActive(false);

        Destroy(gameObject);
    }
}
