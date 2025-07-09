using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    [SerializeField] Transform respawn;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.transform.position = respawn.position;
        }
    }
}
