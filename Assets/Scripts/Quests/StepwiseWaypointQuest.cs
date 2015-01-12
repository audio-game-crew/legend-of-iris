using System;
using System.Collections.Generic;
using UnityEngine;

public class StepwiseWaypointQuest : Quest<StepwiseWaypointQuest, StepwiseWaypointQuestDefinition> {

	public int index = 0;
    private Queue<Waypoint> steps = new Queue<Waypoint>();
    private HashSet<Waypoint> addedWaypoints = new HashSet<Waypoint>();

    public StepwiseWaypointQuest(StepwiseWaypointQuestDefinition definition) : base(definition) { }

    protected override void _Start()
    {
		base._Start();
        FillSteps();
		Next();
	}

    private void FillSteps()
    {
        var lastPosition = Characters.instance.Lucy.transform.position;
        var lastRotation = Characters.instance.Lucy.transform.rotation;
        foreach (var waypoint in definition.waypoints)
        {
            var targetVec = waypoint.transform.position - lastPosition;

            var distance = targetVec.magnitude;
            if (distance < definition.maxStepDistance) // We can just put the final position as the step
                steps.Enqueue(waypoint);
            else
            {
                var stepCount = Mathf.Ceil(distance / definition.maxStepDistance);
                var stepSize = 1 / stepCount;
                for (int i = 1; i <= stepCount; i++)
                {
                    var percentageDone = stepSize * (float)i;
                    Debug.Log(lastPosition + ", " + waypoint);
                    var newWaypointGO = (GameObject)GameObject.Instantiate(definition.WaypointPrefab, 
                        lastPosition + percentageDone * targetVec,
                        Quaternion.RotateTowards(lastRotation, waypoint.transform.rotation, percentageDone));
                    var newWaypoint = newWaypointGO.GetComponent<Waypoint>();
                    if (newWaypoint == null)
                        Debug.LogError("The waypoint prefab doesn't contain a Waypoint component", newWaypointGO);
                    steps.Enqueue(newWaypoint);
                    addedWaypoints.Add(newWaypoint);
                }
            }
            lastPosition = waypoint.transform.position;
            lastRotation = waypoint.transform.rotation;
        }

    }

	private void Next() {
		if (steps.Count == 0) {
			Complete();
			return;
		}
        Waypoint waypoint = steps.Dequeue();
		waypoint.onPlayerEnter += OnPlayerEnter;
        var lucy = Characters.instance.Lucy.GetComponent<LucyController>();
        lucy.GotoLocation(new PositionRotation(waypoint.transform.position, waypoint.transform.rotation));
	}

	private void OnPlayerEnter(Waypoint waypoint, GameObject player) {
		waypoint.onPlayerEnter -= OnPlayerEnter;
        if (addedWaypoints.Contains(waypoint))
        {
            Debug.Log("Destroying waypoint");
            addedWaypoints.Remove(waypoint);
            GameObject.Destroy(waypoint.gameObject);
        }
		Next();
	}

}
