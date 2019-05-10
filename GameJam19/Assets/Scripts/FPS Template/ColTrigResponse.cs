using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ColTrigResponse : MonoBehaviour {

	public mode mymode=mode.TRIGRENTER;
	public string ResponseTag="Player", AudioTag ="Player";
	public AudioSource audi;
	public enum mode {
		COLENTER,COLEXIT,TRIGRENTER,TRIGREXIT};

	public UnityEvent Response;
	string lastTag;

	// Use this for initialization
	void Start () {
		if(!audi)audi=FindObjectOfType<AudioSource>();
	}
	void action(string tag){
		lastTag=tag;
		if(ResponseTag.Equals("") || ResponseTag.Equals(tag))Response.Invoke();
	}

	void OnCollisionEnter(Collision other)
	{
		if(!mymode.Equals(mode.COLENTER)) return;
		action(other.gameObject.tag);
	}
	void OnCollisionExit(Collision other)
	{
		if(!mymode.Equals(mode.COLEXIT)) return;
		action(other.gameObject.tag);
	}
	void OnTriggerEnter(Collider other)
	{
		if(!mymode.Equals(mode.TRIGRENTER)) return;
		action(other.gameObject.tag);
	}
	void OnTriggerExit(Collider other)
	{
		if(!mymode.Equals(mode.TRIGREXIT)) return;
		action(other.gameObject.tag);
	}

	public void kill(string tag){
		if(lastTag.Equals("") || lastTag.Equals(tag))Destroy(gameObject);
		}
	public void playsound(AudioClip clip){
		if(!audi) return;
		if(clip==null) clip=audi.clip;
		if(lastTag.Equals(AudioTag)){
			(audi?audi:FindObjectOfType<AudioSource>()).PlayOneShot(clip);
		}
			//Debug.Log("shot! lastTag>"+lastTag+" "+TagForSound);
		//else Debug.Log("nope! lastTag>"+lastTag+" "+TagForSound);
	}
}
