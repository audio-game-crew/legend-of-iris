using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AmbientArea : MonoBehaviour {

    [System.Serializable]
    public class AmbientSource
    {
        [HideInInspector]
        public string name;
        [HideInInspector]
        public bool initialized;
        [Header("Definition")]
        public List<GameObject> locations;
        public List<AudioClip> clips;
        public float minAudioDistance;
        public float maxAudioDistance;
        [Range(0.5f, 1.5f)]
        public float minPitch;
        [Range(0.5f, 1.5f)]
        public float maxPitch;
        [Range(0, 1)]
        public float volumeModifier = 1f;

        [Header("Spawning")]
        public bool constantLoop;
        public bool randomClipTime = false;
        public bool spawnRandom;
        public float minInterval;
        public float maxInterval;

        [HideInInspector]
        public float currentTimer;
        [HideInInspector]
        public List<AudioPlayer> players = new List<AudioPlayer>();
        [HideInInspector]
        public bool turnedOff = false;

        public AmbientSource Clone()
        {
            return new AmbientSource
            {
                name = this.name,
                initialized = this.initialized,
                locations = this.locations.ToList(),
                clips = this.clips.ToList(),
                minAudioDistance = this.minAudioDistance,
                maxAudioDistance = this.maxAudioDistance,
                minPitch = this.minPitch,
                maxPitch = this.maxPitch,
                volumeModifier = this.volumeModifier,
                constantLoop = this.constantLoop,
                randomClipTime = this.randomClipTime,
                spawnRandom = this.spawnRandom,
                minInterval = this.minInterval,
                maxInterval = this.maxInterval,
            };
        }
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
                s.minPitch = 1f;
                s.maxPitch = 1f;
                s.minInterval = 4f;
                s.maxInterval = 30f;
                s.volumeModifier = 1f;
                s.constantLoop = true;
            }
            s.name = (s.locations.Count > 0 ? s.locations[0].name : "") + ": " + (s.clips.Count > 0 ? s.clips[0].name : "");
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
            {
                foreach (var location in s.locations)
                {
                    PlaceAmbientSound(s, location);
                }
            }
            else
            {
                SetTimer(s);
            }
        }
	}

    void SetTimer(AmbientSource s)
    {
        s.currentTimer = Randomg.Range(s.minInterval, s.maxInterval);
    }

    public void AddAmbientLocation(int sourceID, GameObject location)
    {
        if (ambientSources.Count <= sourceID)
            throw new IndexOutOfRangeException("SourceID out of range for the current AmbientArea");
        var source = ambientSources[sourceID];
        source.locations.Add(location);
        if (source.constantLoop)
            PlaceAmbientSound(source, location);
    }

    public void RemoveAmbientLocation(int sourceID, GameObject location)
    {
        if (ambientSources.Count <= sourceID)
            throw new IndexOutOfRangeException("SourceID out of range for the current AmbientArea");
        var source = ambientSources[sourceID];
        // Check if the given location actually exists. If so remove it.
        if (source.locations.Any(l => l == location))
            source.locations.Remove(location);
        // Remove all audio sources for the location as well
        foreach (var player in source.players.Where(p => p.GameObject == location))
            player.MarkRemovable();
    }

    void PlaceAmbientSound(AmbientSource s, GameObject location)
    {
        AudioPlayer ap = AmbientManager.PlaceAmbientSound(location, s.clips.GetRandom(), s.constantLoop, false);
        ap.SetMinDistance(s.minAudioDistance);
        ap.SetMaxDistance(s.maxAudioDistance);
        ap.SetPitch(Randomg.Range(s.minPitch, s.maxPitch));
        ap.SetOnRemoveListener(delegate()
        {
            AudioPlayer myself = ap;
            s.players.Remove(myself);
        });

        if (s.randomClipTime)
        {
            ap.SetTimePercentage(Randomg.Range01());
        }
        s.players.Add(ap);
    }


	
	// Update is called once per frame
	void Update () {
        // get distance from player to collider
        Vector3 player = PlayerController.instance.transform.position;
        float distance = 0f;
        if (!GetComponent<Collider>().bounds.Contains(player))
        {
            Vector3 closestPoint = GetComponent<Collider>().ClosestPointOnBounds(player);
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
                ConvergeVolume(s, volume);
                
                if (s.spawnRandom)
                {
                    // has no player, and requires random spawns, so let's wait until we can
                    s.currentTimer -= Time.deltaTime;
                    if (s.currentTimer < 0f)
                    {
                        SetTimer(s);
                        PlaceAmbientSound(s, s.locations.GetRandom());
                    }
                }
            }
        }
        // disable ambient
        else
        {
            foreach (var s in ambientSources)
            {
                foreach (var ap in s.players)
                {
                    if (ap != null && !s.turnedOff)
                    {
                        if (ap.GetVolume() > 0.0001f)
                        {
                            ConvergeVolume(s, 0f);
                        }
                        else
                        {
                            ap.SetVolume(0f);
                            s.turnedOff = true;
                        }
                    }
                }
            }
        }
	}

    void ConvergeVolume(AmbientSource s, float targetVolume)
    {
        foreach (var ap in s.players)
        {
            targetVolume *= s.volumeModifier;
            float currentVolume = ap.GetVolume();
            ap.SetVolume(currentVolume + (targetVolume - currentVolume) * Timeg.safeDeltaUnscaled(convergeVolumeSpeed));
        }
    }
}
