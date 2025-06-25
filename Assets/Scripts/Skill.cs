using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    public float speed = 15f;
    public float explodeDelay = 3f;
    public GameObject explosionEffect;

    private Vector3 direction;

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        StartCoroutine(ExplodeAfterDelay());
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explodeDelay);
        Explode();
    }

    void Explode()
    {
        if (explosionEffect)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        // Optional: 충돌 시 조기 폭발 원하면 여기도 Explode() 호출
        // Explode();
    }
}
