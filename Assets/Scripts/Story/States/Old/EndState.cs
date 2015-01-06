using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class EndState : BaseState
{
    public GameObject EndUI;
    public override void Start(Story script)
    {
        EndUI.SetActive(true);
        base.Start(script);
    }

    public override void End(Story script)
    {
        EndUI.SetActive(false);
        base.End(script);
    }
}