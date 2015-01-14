using UnityEngine;
using System.Collections;

public class SpiritController : MonoBehaviour {
	
	private SpiritGeneratorScript generator;
	private float initialSpeed;
	private float speed;
	
	public void Init(SpiritGeneratorScript g, float startingSpeed) {
        //Debug.Log ("we call init spirit controller", gameObject);

		this.generator = g;
		this.initialSpeed = startingSpeed;
		this.speed = startingSpeed;
        var near = this.transform.FindChild("NearSpirit");
        if (near != null)
        {
            var nearTrigger = near.GetComponent<TriggerEventGenerator>();
            if (nearTrigger != null)
            { 
                nearTrigger.TriggerEnter += nearTrigger_TriggerEnter;
                nearTrigger.TriggerExit += nearTrigger_TriggerExit;
            }
        }

        var body = this.transform.FindChild("Body");
        if (body != null)
        {
            var bodyTrigger = body.GetComponent<TriggerEventGenerator>();
            if (bodyTrigger != null)
            {
                bodyTrigger.TriggerEnter += bodyTrigger_TriggerEnter;
                bodyTrigger.TriggerExit += bodyTrigger_TriggerExit;
            }
        }
	}

    void nearTrigger_TriggerEnter(object sender, TriggerEventArgs e)
    {
        var other = e.Trigger;
        if (other.gameObject == Characters.instance.Beorn)
            Debug.LogWarning("Near spirit!");
    }

    void nearTrigger_TriggerExit(object sender, TriggerEventArgs e)
    {
        var other = e.Trigger;
        if (other.gameObject == Characters.instance.Beorn)
        {
            Debug.LogWarning("Near miss spirit!");
            generator.OnNearMissSpirit(this.gameObject);
        }
    } 
	
	public void Stop() {
		speed = 0;
	}
	
	void Update() {
		transform.position += transform.forward*((speed)*Time.deltaTime);
	}

    void bodyTrigger_TriggerEnter(object sender, TriggerEventArgs e)
    {
        if (e.Trigger.tag == "Spirit")
        {
            speed = 0;
            generator.SetActive(false);
        }
        if (e.Trigger.gameObject == Characters.instance.Beorn)
        {
            generator.OnHitSpirit(this.gameObject);
        }
    }

    void bodyTrigger_TriggerExit(object sender, TriggerEventArgs e)
    {
        var other = e.Trigger;
        if (other == generator.spiritLiveArea.collider)
        {
            // If a spirit isn't in the game anymore we take if off the spirit list
            generator.RemoveSpirit(this.gameObject);
            Destroy(gameObject);


        }
        else if (other.tag == "Spirit")
        {
            speed = initialSpeed;
            generator.SetActive(false);
        }
        else if (e.Trigger.gameObject == Characters.instance.Beorn)
        {
            // if the player touches a spirit he is pushed down
            //var directionToPush = transform.position - other.transform.position;
            //directionToPush.Normalize();
            //Debug.Log("directionToPush: " + directionToPush);
            //other.rigidbody.AddForce(directionToPush * 5000);
        }
    }
	
	public void changeSpeed(float newSpeed){
		this.speed = newSpeed;
	}
}
