using System;
using UnityEditor;
using UnityEngine;

public abstract class QuestDefinition : MonoBehaviour {
	abstract public Quest Create();
}
