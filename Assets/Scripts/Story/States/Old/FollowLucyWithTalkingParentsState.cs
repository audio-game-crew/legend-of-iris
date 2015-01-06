using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class FollowLucyWithTalkingParentsState : SimpleFollowLucyState
{
    public AudioClip MotherSound;
    public AudioClip FatherSound;
    public AudioClip BrotherSound;
    //public AudioClip LucySound;

    public GameObject Mother;
    public GameObject Father;
    public GameObject Brother;

    public float MaxRandomDelay = 0.5f;

    private AudioPlayer motherPlayer;
    private AudioPlayer fatherPlayer;
    private AudioPlayer brotherPlayer;
    //private AudioPlayer LucyPlayer;


    public override void Start(Story script)
    {
        motherPlayer = PlayWithRandomDelay(Mother, MotherSound);
        fatherPlayer = PlayWithRandomDelay(Father, FatherSound);
        brotherPlayer = PlayWithRandomDelay(Brother, BrotherSound);
        //LucyPlayer = PlayWithRandomDelay (script.Lucy, LucySound);

        base.Start(script);
    }

    private AudioPlayer PlayWithRandomDelay(GameObject source, AudioClip clip)
    {
        AudioObject ao = new AudioObject(source, clip, 1, Randomg.Range(0, MaxRandomDelay));
        return AudioManager.PlayAudio(ao);
    }

    public override void Update(Story script)
    {

        if (motherPlayer.finished) motherPlayer = PlayWithRandomDelay(Mother, MotherSound);
        if (fatherPlayer.finished) fatherPlayer = PlayWithRandomDelay(Father, FatherSound);
        if (brotherPlayer.finished) brotherPlayer = PlayWithRandomDelay(Brother, BrotherSound);

        base.Update(script);
    }

    public override void End(Story script)
    {
        // TODO: stop the players
        base.End(script);
    }

}