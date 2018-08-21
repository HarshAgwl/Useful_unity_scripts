using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidBaseTemplate : MonoBehaviour {

	protected Animator anim;
	protected Rigidbody rb;
	protected AudioSource aud;

	public float startingHealth;
	public float health;
	public float Health{
		get{
			return health;
		}
		set{
			health=value;
			if(health <= 0f)
				Die();
		}

	}

	protected bool died=false;

	protected void Awake(){
		health = startingHealth;
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		aud = GetComponent<AudioSource>();
	}

	public Collider[] weapons;

	[SerializeField]
	protected AudioClip meleeAttackSfx;

	void Die(){
		if(anim!=null)
			anim.Play("Die");
		if(rb!=null)
			rb.isKinematic = true;
			died = true;
			GetComponent<Collider>().isTrigger = true;
		if(GetComponent<UnityEngine.AI.NavMeshAgent>()){
			GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
		}
		if(GetComponent<TempleChaosPlayerWarrior>()){
			// For Player
			GetComponent<TempleChaosPlayerWarrior>().enabled = false;
		}
		else{
			// For Enemies
			Destroy(gameObject,2f);
		}
	}

	public void TakeHit(float damage){
		Health -= damage;
	}

	void WeaponActive(){
		foreach(Collider weapon in weapons)
			weapon.enabled = true;
	}

	void WeaponInactive(){
		foreach(Collider weapon in weapons)
			weapon.enabled = false;
	}

	 void PlaySound(){
 		if(aud)
 			aud.PlayOneShot(meleeAttackSfx);
 	}

}
