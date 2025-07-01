using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    public GameObject effectPrefab;
    public int poolSize = 5;

    private List<GameObject> pool = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        } 
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject effect = Instantiate(effectPrefab);
            effect.SetActive(false);
            pool.Add(effect);
        }
    }

    public GameObject GetEffect()
    {
        foreach (GameObject effect in pool)
        {
            if (!effect.activeInHierarchy)
            {
                return effect;
            }
        }

        return null;
    }
}
