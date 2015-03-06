using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;

public class TimerManager : MonoBehaviour {

    public static TimerManager instance;
    void Awake()
    {
        instance = this;
    }
    public Text text;
    public float startTime;
    public List<TimedEvent> events = new List<TimedEvent>();
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
        var dirName = Path.GetDirectoryName(instance.fileName);
        if (!Directory.Exists(dirName))
            Directory.CreateDirectory(dirName);
        try
        {
            if (!System.IO.File.Exists(instance.fileName + ".csv"))
                System.IO.File.Create(instance.fileName + ".csv");
            System.IO.File.WriteAllText(instance.fileName + ".csv", output);
        } catch
        {
            // Too bad
        }

    }

	// Use this for initialization
	void Start () {
        startTime = Time.time;

        fileName = "Logs/" + DateTime.Now.ToShortDateString().Replace('/', '-') + " - " + DateTime.Now.ToShortTimeString().Replace(':', '.');
	}

    string GetTimerString() {
        TimeSpan t = TimeSpan.FromSeconds(Time.time - startTime);

        return string.Format("{0:D1}m {1:D2}s",
                        (int)t.TotalMinutes,
                        t.Seconds);
    }
	
	// Update is called once per frame
	void Update () {
        text.text = GetTimerString();
	}
}
