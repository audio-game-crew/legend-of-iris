using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkScript : MonoBehaviour {

    public const GroundType DEFAULT_GROUND_TYPE = GroundType.WOOD;
    
    public Vector3 localWalkingDirection = Vector3.forward;
    public Vector3 distanceToGround = Vector3.down * 2;
    public float feetReachForward = 0.4f;
    public float feetGapWidth = 0.2f;
    public float distancePerStep = 0.4f;
    [Header("Naturality Extension")]
    [Range(0.0F, 1.0F)]
    [Tooltip("Adds a random number to each step length, from a uniform distribution with range [-r, r]")]
    public float randomness = 0.01f;

    [Header("Ground type")]
    public List<GoundTypeToSound> sounds;
    public GroundType currentGroundType;
    public FootStepScript currentFootsteps;

    [System.Serializable]
    public class GoundTypeToSound
    {
        public GroundType ground;
        public FootStepScript sounds;
    }


    private float toStep = 0f;
    private Foot passingFoot = Foot.LEFT;
    private Vector3 previousPosition = Vector3.zero;
    private Vector3 direction;
    private bool stopped = true;

    private enum Foot
    {
        LEFT, RIGHT
    }

	void Start () 
    {
        previousPosition = transform.position;
        direction = transform.TransformDirection(localWalkingDirection).normalized;
        toStep = distancePerStep / 0.5f;
        currentGroundType = DEFAULT_GROUND_TYPE;
	}
	
	void Update () 
    {
        DetermineGroundType();

        Vector3 positionNow = transform.position;
        Vector3 moved = positionNow - previousPosition;
        float distanceMoved = moved.magnitude;
        toStep -= distanceMoved;
        previousPosition = positionNow;

        if (distanceMoved < 0.01f)
        {
            // we haven't moved, play a step once and then leave it at that
            if (!stopped)
            {
                resetStepTimer();
                playFootStep();
                stopped = true;
            }
        } 
        else 
        {
            if (toStep < -distancePerStep) 
            {
                // we took a big leap... just pretend nothing happened, we might have been teleported
                // thus we dont want thousand step sounds
                resetStepTimer();
            } 
            else if (toStep < 0f)
            {
                // we did a step
                playFootStep();
                setNewFootstepTimer();
            }

            stopped = false;
            direction = (moved.normalized * 0.5f + direction.normalized * 0.5f).normalized;
        }

	}

    void DetermineGroundType()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position - Vector3.up * 100f, Vector3.up, out hit, 200f, LayerMask.GetMask("GroundType")))
        {
            GroundTypeArea gta = hit.collider.GetComponent<GroundTypeArea>();
            currentGroundType = gta.groundType;
        }
        else
        {
            currentGroundType = DEFAULT_GROUND_TYPE;
        }

        foreach (GoundTypeToSound gtts in sounds)
        {
            if (gtts.ground == currentGroundType)
            {
                currentFootsteps = gtts.sounds;
            }
        }
    }

    void setNewFootstepTimer()
    {
        toStep += distancePerStep + Randomg.Symmetrical(randomness);
    }

    void resetStepTimer()
    {
        toStep = distancePerStep / 2f + Randomg.Symmetrical(randomness);
    }

    void playFootStep()
    {
        Vector3 side = direction.rotatey((passingFoot == Foot.LEFT) ? 90 : -90);

        GameObject go = Pool.get(currentFootsteps.gameObject, transform.position + distanceToGround + direction * feetReachForward + side * feetGapWidth * 0.5f);
        go.GetComponent<FootStepScript>().Initialize();
        SoundSystemManager.HandleAudioSource(go);

        passingFoot = (passingFoot == Foot.LEFT) ? Foot.RIGHT : Foot.LEFT;
    }
}
