using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class LucyRingingBellState : BaseState
{
    public float MaxRandomBellDelay = 0.5f;

    protected AudioPlayer LucyBellPlayer;

    public override void Start(Story script)
    {
        LucyBellPlayer = PlayWithRandomDelay(script.Lucy, script.LucyBell);

        base.Start(script);
    }

    public override void Update(Story script)
    {
        if (LucyBellPlayer.finished)
            LucyBellPlayer = PlayWithRandomDelay(script.Lucy, script.LucyBell);
    }

    private AudioPlayer PlayWithRandomDelay(GameObject source, AudioClip clip)
    {
        AudioObject ao = new AudioObject(source, clip, 1, Randomg.Range(0, MaxRandomBellDelay));
        return AudioManager.PlayAudio(ao);
    }
}
