using UnityEngine;
using System.Collections;

public class LevelSwitcher : MonoBehaviour {

    private static LevelSwitcher instance;
    void Awake()
    {
        instance = this;
    }

    public static void ActivateLevel(int index)
    {
        instance.activateLevel(index);
    }

    public int startIndex = 0;
    public int indexOffset = 1;
    void Start()
    {
        activateLevel(startIndex);
    }

    private void activateLevel(int index)
    {
        for (int i = indexOffset; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            bool enable = (i - indexOffset) == index;
            if (child.activeSelf != enable)
                child.SetActive(enable);
        }
    }
}
