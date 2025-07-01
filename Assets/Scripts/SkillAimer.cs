using UnityEngine;
using System.Collections;

public class SkillAimer : MonoBehaviour
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
        if (Input.GetMouseButtonDown(0)&& isOnCooldown == false)
        {
            StartCoroutine(AimAndFire());
        }
    }

    IEnumerator AimAndFire()
    {
        rangeInstance = Instantiate(rangeIndicatorPrefab);

        // �ٴ� ��� (y=0 ����)
        Plane floorPlane = new Plane(Vector3.up, Vector3.zero);

        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (floorPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter); // ���콺 ��ġ�� �ٴ� ����� ����

                Vector3 from = firePoint.position;
                Vector3 dir = hitPoint - from;
                dir.y = 0; // ���� ���⸸ ���

                if (dir.magnitude > maxRange)
                {
                    dir = dir.normalized * maxRange;
                }

                Vector3 targetPos = from + dir;
                rangeInstance.transform.position = targetPos;
            }

            yield return null;
        }

        // ���콺�� ���� �߻�
        GameObject skillObj = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);

        skillObj.GetComponent<Skill>().SkillBoom(rangeInstance.transform.position);

        Destroy(rangeInstance);
    }


}
