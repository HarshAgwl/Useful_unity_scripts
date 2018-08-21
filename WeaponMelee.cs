using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : MonoBehaviour {

	private Collider weapon;
	[SerializeField]
	private float damage;
	private float playerHealth;

	void Awake(){
		
	}
	void Start () {
		weapon = GetComponent<Collider>();
		weapon.enabled = false;
	}
	
	void Update () {
	}


	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			Manager.instance.player.GetComponent<TempleChaosPlayerWarrior>().TakeHit(damage);
			
		}
		if(other.tag=="Enemy"){
			other.gameObject.GetComponent<EnemyScriptNew>().TakeHit(damage);
			
		}
	}
}
