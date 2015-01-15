using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class TimerManager : MonoBehaviour {

    public static TimerManager instance;
    void Awake()
    {
        instance = this;
    }
    public Text text;
    public float startTime;
    public List<TimedEvent> events;
    public string fileName;

    public class TimedEvent
    {
        public float time;
        public string niceTime;
        public string eventName;

        public override string ToString()
        {
            return time + ";" + niceTime + ";" + eventName;
        }
    }

    public static void RegisterEvent(string eventName) {
        instance.events.Add(new TimedEvent() {
            time = Time.time - instance.startTime,
            niceTime = instance.GetTimerString(),
            eventName = eventName
        });
        string output = "";
        foreach (TimedEvent t in instance.events) {
            output += t + "\n";
        }
        System.IO.File.WriteAllText(instance.fileName + ".csv", output);

    }

	// Use this for initialization
	void Start () {
        startTime = Time.time;

        fileName = new DateTime().ToShortDateString() + " - " + new DateTime().ToShortTimeString();
	}

    string GetTimerString() {
        TimeSpan t = TimeSpan.FromSeconds(Time.time - startTime);

        return string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
    }
	
	// Update is called once per frame
	void Update () {
        text.text = GetTimerString();
	}
}
