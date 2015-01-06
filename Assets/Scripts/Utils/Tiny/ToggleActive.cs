using UnityEngine;
using System.Collections;

public class ToggleActive : MonoBehaviour {

    public GameObject toToggle;
    public void Toggle()
    {
        toToggle.SetActive(!toToggle.activeInHierarchy);
    }
    public void SetActive()
    {
        toToggle.SetActive(true);
    }
    public void SetInactive()
    {
        toToggle.SetActive(false);
    }
}
