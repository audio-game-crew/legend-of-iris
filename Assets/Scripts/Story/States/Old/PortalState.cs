using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class PortalState : LucyExplainingState
{
    public float PortalSoundDuration = 5f;
    private float PortalSoundPlayTime = 0f;

    public override void Start(Story script)
    {
        PortalSoundPlayTime = 0f;
        base.Start(script);
    }

    public override void Update(Story script)
    {
        PortalSoundPlayTime += Time.deltaTime;
        if (PortalSoundPlayTime > PortalSoundDuration)
            script.LoadState(NextState);
        base.Update(script);
    }
}
