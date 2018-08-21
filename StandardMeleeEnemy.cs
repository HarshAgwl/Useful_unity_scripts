using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyScriptNew : HumanoidBaseTemplate {

	private NavMeshAgent nav;
	private GameObject target;
	[SerializeField]
	private float range=2f;
	[SerializeField]
	private float delayBetweenAttacks=2f;

	private bool targetInAttackRange=false;
	private int currentTargetType = -1;

	[SerializeField]
	private float seeRange = 27.5f;

	[SerializeField]
	private Transform raycastTransform;

	private Vector3 dirToUnit;


	void Awake(){
		base.Awake();
	}

	// Use this for initialization
	void Start () {
		// player = Manager.instance.player; 
		nav = GetComponent<NavMeshAgent>();
		StartCoroutine(Attack());
	}
	
	// Update is called once per frame
	void Update () {
		if(!died && target!=null)
			nav.SetDestination(target.transform.position);
		
		if((transform.position-player.transform.position).sqrMagnitude <= range*range){
			targetInAttackRange = true;
		}
		else{
			targetInAttackRange = false;
		}
	}

	IEnumerator Attack(){
		if(targetInAttackRange && !died){
			anim.Play("Attack");

			yield return new WaitForSeconds(delayBetweenAttacks);
		}
		yield return null;
		StartCoroutine(Attack());
	}

	voin OnTriggerEnter(Collider other){
		if(target==null){
			if(other.tag=="Peasant"){
				if(CheckIfVisible(other.gameObject)){
					target = other.gameObject;
					currentTargetType = 0;
				}
			}
		}
		if(currentTargetType==0 || currentTargetType==-1){
			if(other.tag=="Player" || other.tag="PlayerAlly"){
				if(CheckIfVisible(other.gameObject)){
					target = other.gameObject;
					currentTargetType = 1;
				}
			}
		}
	}

	bool CheckIfVisible(GameObject unit){
		dirToUnit = unit.transform.position - raycastTransform.position;
		if(Vector3.Angle(raycastTransform.forward,dirToUnit) <= eyeRangeAngle){
			RaycastHit hit;
			if(Physics.Raycast(raycastTransform.position,dirToUnit,out hit,seeRange)){
				//Keeping track of unit's position
				// targetPosition = unit.transform.position;
				// seeingEnemy = true;
				// spottedEnemy = true;
				//Turn enemy towards player
				// transform.rotation = Quaternion.LookRotation(new Vector3(unit.transform.position.x-transform.position.x,0f,unit.transform.position.z-transform.position.z));
				return true;
			}
		}
		return false;
	}

	// void OnDestroy(){
	// 	SpawnManager.enemiesOnScreen--;
	// }
}
