using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public PhysicsMaterial2D highBounceMat;   // bounciness = 3
    public PhysicsMaterial2D normalBounceMat; // bounciness = 1

    private Collider2D col;
    private bool isResetting = false;

    void Start()
    {
        col = GetComponent<Collider2D>();
        col.sharedMaterial = highBounceMat; // √≥¿Ω¿∫ ∆¢∞‘
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            col.sharedMaterial = normalBounceMat;

            if (!isResetting)
            {
                StartCoroutine(ResetBounceAfterDelay());
            }
        }
    }

    IEnumerator ResetBounceAfterDelay()
    {
        isResetting = true;
        yield return new WaitForSeconds(2f); // 2√  æ» ¥Í¿∏∏È
        col.sharedMaterial = highBounceMat;
        isResetting = false;
    }
}
