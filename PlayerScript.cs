using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	private Rigidbody rb;
	private ContactPoint contactPoint;
	private bool hasContactPoint = false;
	private GameObject parent;

	[SerializeField]
	private float movementSpeed = 20f;

	[SerializeField]
	private float throwForce = 350f;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(hasContactPoint){
			Debug.DrawRay(transform.position, contactPoint.normal*10f,Color.green);
			if(Input.GetKeyDown(KeyCode.Space)){
				rb.AddForce(contactPoint.normal*throwForce);
			}
		}
		if(transform.parent!=null){
			transform.parent.transform.Rotate(new Vector3(0,0,-Input.GetAxis("Horizontal")*movementSpeed*Time.deltaTime),Space.World);
		}

	}

	void OnCollisionEnter(Collision other){
		if(other.collider.tag=="Planet"){
			rb.useGravity = false;
			rb.velocity = new Vector3(0f,0f,0f);
			transform.parent = other.collider.gameObject.transform;
		}
	}

	void OnCollisionStay(Collision other){
		if(other.collider.tag=="Planet"){
			rb.useGravity = false;
			hasContactPoint = true;
			contactPoint =  other.contacts[0];
			// Debug.DrawRay(transform.position, other.contacts[0].normal*10f, Color.green);
		}
	}

	void OnCollisionExit(Collision other){
		if(other.collider.tag=="Planet"){
			Debug.Log("exited");
			rb.useGravity = true;
			transform.parent = null;
		}
	}
}
