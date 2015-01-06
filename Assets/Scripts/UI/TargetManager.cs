using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetManager : MonoBehaviour
{
    private static TargetManager instance;
    void Awake()
    {
        instance = this;
    }

    public static void SetTarget(GameObject targetObject)
    {
        instance.target = targetObject;
    }

    public static void SetSource(GameObject sourceObject)
    {
        instance.source = sourceObject;
    }

    public GameObject source;
    public GameObject target;
}
