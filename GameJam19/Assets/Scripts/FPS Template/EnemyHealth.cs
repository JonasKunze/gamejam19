using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Serialization;

public class EnemyHealth : Photon.MonoBehaviour {

	public int startingHealth = 10;
	public int currentHealth;
	public float sinkSpeed = 0.12f;
	public AudioClip deathClip;
	public AudioClip hurtClip;
    //public Animator anim;

    public int karmaValue = 5;

	private AudioSource audioSource;
	private CapsuleCollider capsuleCollider;
	private SphereCollider sphereCollider;
	//private IKControl ikControl;
	[FormerlySerializedAs("isSinking")] public bool isDead;
	private bool damaged;

	// Called when script awake in editor
	void Awake() {
		audioSource = GetComponent<AudioSource>();
		capsuleCollider = GetComponentInChildren<CapsuleCollider>();
		sphereCollider =GetComponentInChildren<SphereCollider>();
		currentHealth = startingHealth;
	}

	// Update is called once per frame
	void Update() {

		if (isDead)
		{
			transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, 0.1f, 0.1f);
			transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
		}
	}

	// The RPC function to let the player take damage
	[PunRPC]
	public void TakeDamage(int amount, string enemyName) {
		currentHealth -= amount;

		//anim.SetTrigger("IsHurt");

		if (audioSource)
		{
			audioSource.clip = hurtClip;
			audioSource.Play();
		}
		
		if (currentHealth <= 0) {
            photonView.RPC("Death", PhotonTargets.All, enemyName);
		}
	}

	// The RPC function for enemy death
	[PunRPC]
	void Death(string enemyName) {
		capsuleCollider.isTrigger = true;
		sphereCollider.isTrigger = true;

		//anim.SetTrigger("IsDead");

		audioSource.clip = deathClip;
		audioSource.Play();

        //ikControl.enabled = false;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players) {
            if (player.name.Equals(enemyName)) {
                player.GetComponent<ConvictionController>().GenerateConviction(false, karmaValue);
            }
        }

		StartCoroutine("StartSinking", 2.5f);
	}


	// The RPC function to start sink the enemy game object
	[PunRPC]
	IEnumerator StartSinking(float time) {
		yield return new WaitForSeconds(time);
		GetComponent<Rigidbody>().isKinematic = true;
		isDead = true;
        StartCoroutine(DestroyEnemyObject(2f));
	}

	// The RPC function to destory enemy game object
	[PunRPC]
	IEnumerator DestroyEnemyObject(float time) {
		yield return new WaitForSeconds(time);
		//if (transform.parent.name.Equals("Pepegas")) {
		//EnemyManager.deadPepegas.Enqueue(gameObject);
		capsuleCollider.isTrigger = false;
		sphereCollider.isTrigger = false;
		GetComponent<Rigidbody>().isKinematic = false;
		//isDead = false;
		gameObject.SetActive(false);
		currentHealth = startingHealth;
		//} else {
		//Destroy(gameObject);
		//}
        
	}
	
/*	// Synchronize data on the network
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext(currentHealth);
		} else {
			currentHealth = (int)stream.ReceiveNext();
		}
	}*/

}