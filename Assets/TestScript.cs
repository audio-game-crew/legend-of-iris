using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TestScript : MonoBehaviour {

    public AudioClip clip;
    public Gradient gradient;
    public UnityEvent eventListeners;
    public AnimationCurve animCurve;
    [Header("Text")]
    [Tooltip("shizzles")]
    [Multiline(5)]
    public string multipleLines;
    [TextArea]
    public string textArea;
    [Range(0, 10)]
    public int slider;

	// Use this for initialization
    void Start()
    {
        AudioObject ao = new AudioObject(gameObject, clip, 1f, 2f);
        AudioManager.PlayAudio(ao);
        ao = new AudioObject(gameObject, clip, 1f);
        //AudioManager.PlayAudio(ao);
        PlayConversation();
	}

    void PlayConversation()
    {
        var cp = ConversationManager.PlayConversation("T2.1", delegate() {
            // Debug.Log("Conversation ended!!");
        });

        cp.SetOnMessageStartListener(delegate(int index) {
            // Debug.Log("Started message #" + index);
        });

        cp.SetOnMessageEndListener(delegate(int index) {
            // Debug.Log("Ended message #" + index);
        });
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            PlayConversation();
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
