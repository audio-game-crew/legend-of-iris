using UnityEngine;
using System.Collections;
using System;

public class TriggerEventGenerator : MonoBehaviour {
    public event EventHandler<TriggerEventArgs> TriggerEnter;
    public event EventHandler<TriggerEventArgs> TriggerExit;

    void OnTriggerEnter(Collider other)
    {
        if (TriggerEnter != null)
            TriggerEnter(this, new TriggerEventArgs(other));
    }

    void OnTriggerExit(Collider other)
    {
        if (TriggerExit != null)
        {
            TriggerExit(this, new TriggerEventArgs(other));
        }
    }
}
