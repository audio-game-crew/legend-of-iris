using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public abstract class BaseState
{
    public virtual void Start(Story script) { }
    public virtual void Update(Story script) { }
    public virtual void End(Story script) { }
    public virtual void PlayerEnteredTrigger(Collider collider, Story script) { }
    public virtual void PlayerExitTrigger(Collider collider, Story script) { }
}
