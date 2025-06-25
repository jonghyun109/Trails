using UnityEngine;
using System.Collections;

public class SkillAimer : MonoBehaviour
{
    public GameObject skillPrefab;              // ← 이름 변경
    public GameObject rangeIndicatorPrefab;
    public Transform firePoint;
    public float maxRange = 8f;

    private GameObject rangeInstance;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AimAndFire());
        }
    }

    IEnumerator AimAndFire()
    {
        if (rangeIndicatorPrefab)
            rangeInstance = Instantiate(rangeIndicatorPrefab);

        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 from = firePoint.position;
                Vector3 dir = hit.point - from;
                dir.y = 0;

                if (dir.magnitude > maxRange)
                    dir = dir.normalized * maxRange;

                Vector3 targetPos = from + dir;

                if (rangeInstance)
                    rangeInstance.transform.position = targetPos;
            }

            yield return null;
        }

        if (rangeInstance)
        {
            Vector3 shootDir = (rangeInstance.transform.position - firePoint.position).normalized;

            GameObject skillObj = Instantiate(skillPrefab, firePoint.position, Quaternion.identity);
            skillObj.GetComponent<Skill>().Launch(shootDir);

            Destroy(rangeInstance);
        }
    }
}
