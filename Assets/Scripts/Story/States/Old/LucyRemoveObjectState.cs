using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class LucyRemoveObjectState : BaseState
{
    public GameObject TargetObject;
    public BaseState NextState;

    public override void Update(Story script)
    {
        TargetObject.SetActive(false);
        script.LoadState(NextState);
    }

}
