using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [Header("boom, warning, strike")]
    public List<GameObject> effectPrefabs;
    public int poolSize = 5;

    private List<List<GameObject>> pools = new List<List<GameObject>>();

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
        foreach (GameObject prefab in effectPrefabs)
        {
            List<GameObject> newPool = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                newPool.Add(obj);
            }
            pools.Add(newPool);
        }
    }

    public GameObject GetEffect(int index)
    {
        if (index < 0 || index >= pools.Count) return null;

        foreach (GameObject obj in pools[index])
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        return null;
    }
}
