using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Pool instance;
    private static Dictionary<GameObject, List<GameObject>> pool = new Dictionary<GameObject, List<GameObject>>();
    private Transform inactivated;

    void Awake()
    {
        instance = this;

        GameObject go = new GameObject("Inactivated");
        inactivated = go.transform;
        inactivated.parent = transform;
        go.SetActive(false);
    }

    public static GameObject get(GameObject original)
    {
        return get(original, Vector3.zero);
    }

    public static GameObject get(GameObject original, Vector3 position)
    {
        return get(original, position, Quaternion.identity);
    }

    public static GameObject get(GameObject original, Quaternion rotation)
    {
        return get(original, Vector3.zero, rotation);
    }

    public static GameObject get(GameObject original, Vector3 position, Vector3 scale)
    {
        return get(original, position, Quaternion.identity, scale);
    }

    public static GameObject get(GameObject original, Vector3 position, Quaternion rotation)
    {
        return get(original, position, rotation, Vector3.one);
    }

    public static GameObject get(GameObject original, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        return get(instance.transform.root, original, position, rotation, scale);
    }


    public static GameObject get(Transform newParent, GameObject original)
    {
        return get(newParent, original, Vector3.zero);
    }

    public static GameObject get(Transform newParent, GameObject original, Vector3 position)
    {
        return get(newParent, original, position, Quaternion.identity);
    }

    public static GameObject get(Transform newParent, GameObject original, Quaternion rotation)
    {
        return get(newParent, original, Vector3.zero, rotation);
    }

    public static GameObject get(Transform newParent, GameObject original, Vector3 position, Vector3 scale)
    {
        return get(newParent, original, position, Quaternion.identity, scale);
    }

    public static GameObject get(Transform newParent, GameObject original, Vector3 position, Quaternion rotation)
    {
        return get(newParent, original, position, rotation, Vector3.one);
    }

    public static GameObject get(Transform newParent, GameObject original, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        if (!pool.ContainsKey(original))
        {
            pool.Add(original, new List<GameObject>());
        }

        GameObject go;
        if (pool[original].Count > 0)
        {
            go = pool[original].Pop();
        }
        else
        {
            go = (GameObject) GameObject.Instantiate(original);
            PoolableObject po = go.AddComponent<PoolableObject>();
            po.parent = original;
        }

        go.transform.parent = newParent;

        //if (original.transform.localScale != Vector3.one || scale != Vector3.one)
            go.transform.localScale = original.transform.localScale.times(scale);

        //if (original.transform.localPosition != Vector3.zero || position != Vector3.zero)
            go.transform.localPosition = original.transform.localPosition + position;

        //if (original.transform.rotation != Quaternion.identity || rotation != Quaternion.identity)
            go.transform.rotation = original.transform.rotation * rotation;

        return go;
    }

    public static void put(GameObject poolable)
    {
        PoolableObject po = poolable.GetComponent<PoolableObject>();
        if (po != null)
        {
            pool[po.parent].Add(poolable);
            poolable.transform.parent = instance.inactivated;
        }
    }
}

public class PoolableObject : MonoBehaviour
{
    public GameObject parent;
}