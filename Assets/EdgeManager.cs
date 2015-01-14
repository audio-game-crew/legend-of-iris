using UnityEngine;
using System.Collections;
using System;

public class EdgeManager : MonoBehaviour {
    public event EventHandler<TriggerEventArgs> EnterEdge;
    public event EventHandler<TriggerEventArgs> ExitEdge;
    public event EventHandler<TriggerEventArgs> PlayerEnterEdge;
    public event EventHandler<TriggerEventArgs> PlayerExitEdge;

    public static EdgeManager instance;

    public void Awake()
    {
        instance = this;
    }

    public bool InEdge { get; private set; }

    public void EdgeTriggerEnter(Collider trigger, Collider other)
    {
        InEdge = true;
        if (EnterEdge != null)
            EnterEdge(trigger, new TriggerEventArgs(other));
        var player = Characters.instance.Beorn;
        if (player != null && other.gameObject == player)
        {
            if (PlayerEnterEdge != null)
                PlayerEnterEdge(trigger, new TriggerEventArgs(other));
        }
    }

    public void EdgeTriggerExit(Collider trigger, Collider other)
    {
        InEdge = false;
        if (ExitEdge != null)
            ExitEdge(trigger, new TriggerEventArgs(other));
        var player = Characters.instance.Beorn;
        if (player != null && other.gameObject == player)
        {
            if (PlayerExitEdge != null)
                PlayerExitEdge(trigger, new TriggerEventArgs(other));
        }
    }
}
