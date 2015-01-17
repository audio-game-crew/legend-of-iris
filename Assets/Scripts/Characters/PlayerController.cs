using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    void Awake()
    {
        instance = this;
    }

    public Transform camerasContainer;
	public AudioClip growlSound;
	private AudioPlayer growlPlayer;

    public string EdgeConversation;
    public string EdgeSaveConversation;

	private bool onFire = false;
    private ConversationPlayer edgeConversationPlayer;
    public bool InEdge = false;

    public event EventHandler<TriggerEventArgs> TriggerEntered;
    public event EventHandler<TriggerEventArgs> TriggerExit;

    public bool LockMovement = false;

    void Start()
    {
        EdgeManager.instance.PlayerEnterEdge += instance_PlayerEnterEdge;
        EdgeManager.instance.PlayerExitEdge += instance_PlayerExitEdge;
    }

    void instance_PlayerExitEdge(object sender, TriggerEventArgs e)
    {
        InEdge = false;
        if (edgeConversationPlayer != null)
            edgeConversationPlayer.Skip();
    }

    void instance_PlayerEnterEdge(object sender, TriggerEventArgs e)
    {
        InEdge = true;
        StartEdgeConversation();
    }

    private void StartEdgeConversation()
    {
        if (!string.IsNullOrEmpty(EdgeConversation) && (edgeConversationPlayer == null || edgeConversationPlayer.IsFinished()))
        {
            edgeConversationPlayer = ConversationManager.GetConversationPlayer(EdgeConversation);
            edgeConversationPlayer.Start();
        }
    }

    void Update()
    {
        if (InEdge && edgeConversationPlayer != null && edgeConversationPlayer.IsFinished())
            StartEdgeConversation();
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        BaseControls c = ControlsManager.current;
        if (SettingsManager.instance.IsSettingsShown()) return;

        if (ControlsManager.instance.NeedsReset())
        {
            Vector3 euler = camerasContainer.localEulerAngles;
            camerasContainer.localEulerAngles = euler.setxz(0, 0);
            ControlsManager.instance.ResetDone();
        }

        if (!LockMovement)
            rigidbody.MovePosition(rigidbody.position + c.GetMove(transform.localPosition) * (Input.GetKey(KeyCode.LeftShift) ? 4f : 1f));
        camerasContainer.localRotation *= c.GetRotation(camerasContainer.localRotation);
	}

	public void useGrowl() {
		AudioObject ao = new AudioObject(this.gameObject, growlSound);
		growlPlayer = AudioManager.PlayAudio(ao);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (TriggerEntered != null)
            TriggerEntered(this, new TriggerEventArgs(other));
    }

    void OnTriggerExit(Collider other)
    {
        if (TriggerExit != null)
            TriggerExit(this, new TriggerEventArgs(other));
    }

    public void MoveToLocation(PositionRotation location, float moveTime = 0)
    {
        // TODO: make this smooth
        this.transform.position = location.Position;
        this.transform.rotation = location.Rotation;
    }

	public void setOnFire(bool state) {
		onFire = state;
	}

	public bool isOnFire() {
		return onFire;
	}
}
