﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AmbientArea : MonoBehaviour {

    [System.Serializable]
    public class AmbientSource
    {
        [HideInInspector]
        public bool initialized;
        [Header("Definition")]
        public GameObject location;
        public List<AudioClip> clips;
        public float minAudioDistance;
        public float maxAudioDistance;
        [Range(0, 1)]
        public float volumeModifier = 1f;

        [Header("Spawning")]
        public bool constantLoop;
        public bool spawnRandom;
        public float minInterval;
        public float maxInterval;

        [HideInInspector]
        public float currentTimer;
        [HideInInspector]
        public AudioPlayer player;
        [HideInInspector]
        public bool turnedOff = false;
    }

    void OnDrawGizmos() // hacky way to initialize ambient sources properly
    {
        foreach (var s in ambientSources)
        {
            if (!s.initialized)
            {
                s.initialized = true;
                s.minAudioDistance = 1f;
                s.maxAudioDistance = 500f;
                s.minInterval = 4f;
                s.maxInterval = 30f;
                s.volumeModifier = 1f;
                s.constantLoop = true;
            }
        }
    }

    public enum ActiveCondition
    {
        WHEN_PAUSED, WHEN_PLAYING
    }

    public bool ambientEnabled = true;
    public ActiveCondition activeCondition = ActiveCondition.WHEN_PLAYING;
    public float maxColliderDistance = 10f;
    public float convergeVolumeSpeed = 2f;
    public List<AmbientSource> ambientSources = new List<AmbientSource>();

	// Use this for initialization
	void Start () {
        foreach (var s in ambientSources)
        {
            if (s.constantLoop)
                PlaceAmbientSound(s);
            else
                SetTimer(s);
        }
	}

    void SetTimer(AmbientSource s)
    {
        s.currentTimer = Randomg.Range(s.minInterval, s.maxInterval);
    }

    void PlaceAmbientSound(AmbientSource s)
    {
        s.player = AmbientManager.PlaceAmbientSound(s.location, s.clips.GetRandom(), s.constantLoop, false);
        s.player.SetMinDistance(s.minAudioDistance);
        s.player.SetMaxDistance(s.maxAudioDistance);
        s.player.SetOnRemoveListener(delegate()
        {
            AmbientSource a = s;
            a.player = null;
        });
    }
	
	// Update is called once per frame
	void Update () {
        // get distance from player to collider
        Vector3 player = PlayerController.instance.transform.position;
        float distance = 0f;
        if (!collider.bounds.Contains(player))
        {
            Vector3 closestPoint = collider.ClosestPointOnBounds(player);
            distance = (player - closestPoint).magnitude;
        }

        // check if we should be enabled
        bool enabled;
        if (PauseManager.paused)
        {
            enabled = ambientEnabled && activeCondition == ActiveCondition.WHEN_PAUSED;
        }
        else
        {
            enabled = ambientEnabled && activeCondition == ActiveCondition.WHEN_PLAYING;
        }

        // set volume if close enough
        if (enabled && distance < maxColliderDistance)
        {
            float volume = (maxColliderDistance - distance) / maxColliderDistance;

            foreach (var s in ambientSources)
            {
                s.turnedOff = false;
                // if it has a player, modify its volume
                if (s.player != null)
                {
                    // let's converge to target volume
                    ConvergeVolume(s, volume);
                }
                else if (s.spawnRandom)
                {
                    // has no player, and requires random spawns, so let's wait until we can
                    s.currentTimer -= Time.deltaTime;
                    if (s.currentTimer < 0f)
                    {
                        SetTimer(s);
                        PlaceAmbientSound(s);
                    }
                }
            }
        }
        // disable ambient
        else
        {
            foreach (var s in ambientSources)
            {
                if (s.player != null && !s.turnedOff)
                {
                    if (s.player.GetVolume() > 0.0001f)
                    {
                        ConvergeVolume(s, 0f);
                    }
                    else
                    {
                        s.player.SetVolume(0f);
                        s.turnedOff = true;
                    }
                }
            }
        }
	}

    void ConvergeVolume(AmbientSource s, float targetVolume)
    {
        targetVolume *= s.volumeModifier;
        float currentVolume = s.player.GetVolume();
        s.player.SetVolume(currentVolume + (targetVolume - currentVolume) * Timeg.safeDeltaUnscaled(convergeVolumeSpeed));
    }
}