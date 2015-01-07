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

        ConversationManager.PlayConversation("T2.1");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            ConversationManager.PlayConversation("T2.1");
        }

        if (Input.GetKeyUp(KeyCode.F1))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}
}
