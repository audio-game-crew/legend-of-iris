using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TriggerEventArgs : EventArgs
{
    public Collider Trigger;

    public TriggerEventArgs(Collider trigger)
    {
        this.Trigger = trigger;
    }
}
