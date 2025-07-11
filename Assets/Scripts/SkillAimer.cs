using UnityEngine;
using System.Collections;
using Photon.Pun;

public class SkillAimer : MonoBehaviourPun
{
    public GameObject skillPrefab;
    public GameObject rangeIndicatorPrefab;
    public Transform firePoint;
    public float maxRange = 8f;
    public float skillCoolTime = 3f;

    private GameObject rangeInstance;
    private bool isOnCooldown = false;

    void Update()
    {

        if (Input.GetMouseButtonDown(0)&& isOnCooldown == false && photonView.IsMine)
        {
            StartCoroutine(AimAndFire());
        }
    }

    IEnumerator AimAndFire()
    {
        rangeInstance = Instantiate(rangeIndicatorPrefab);

        // 바닥 평면 (y=0 기준)
        Plane floorPlane = new Plane(Vector3.up, Vector3.zero);

        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (floorPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter); // 마우스 위치와 바닥 평면의 교점

                Vector3 from = firePoint.position;
                Vector3 dir = hitPoint - from;
                dir.y = 0; // 수평 방향만 사용

                if (dir.magnitude > maxRange)
                {
                    dir = dir.normalized * maxRange;
                }

                Vector3 targetPos = from + dir;
                rangeInstance.transform.position = targetPos;
            }

            yield return null;
        }

        //플레이어 1 Boom
        if(PhotonNetwork.IsMasterClient)
        {
            GameObject skillObj = PhotonNetwork.Instantiate("Shooter", firePoint.position, Quaternion.identity);

            if (skillObj.GetComponent<PhotonView>().IsMine)
            {
                skillObj.GetComponent<PhotonView>().RPC("SkillBoom", RpcTarget.All, rangeInstance.transform.position);
            }
            Destroy(rangeInstance);
        }
        //플레이어 2 Lightning
        else
        {
            Vector3 strikePos = rangeInstance.transform.position;

            GameObject lightning = PhotonNetwork.Instantiate("Lightning", strikePos + Vector3.up * 10f, Quaternion.identity); // 공중에서 떨어지게 생성

            if (lightning.GetComponent<PhotonView>().IsMine)
            {
                lightning.GetComponent<PhotonView>().RPC("StrikeLightning", RpcTarget.All, strikePos);
            }

            Destroy(rangeInstance);
        }

    }


}
