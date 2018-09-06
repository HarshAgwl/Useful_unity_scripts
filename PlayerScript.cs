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

	[SerializeField]
	private CameraMovement cameraScript;

	[SerializeField]
	private GameObject[] pathBalls;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
		foreach(GameObject ball in pathBalls){
				ball.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(rb.velocity);
		if(hasContactPoint){
			Debug.Log(contactPoint.normal.magnitude*0.02f*350f);
			Debug.DrawRay(transform.position, contactPoint.normal*10f,Color.green);
			if(Input.GetKeyDown(KeyCode.Space)){
				rb.AddForce(contactPoint.normal*throwForce);
			}
			for(int i=0;i<pathBalls.Length;i++){
				pathBalls[i].transform.position = new Vector3(transform.position.x + ((contactPoint.normal*0.02f*350f).x)*(0.125f*i), transform.position.y + ((contactPoint.normal*0.02f*350f).y)*(0.125f*i) + (-9.81f*(0.125f*i)*(0.125f*i)*0.5f), pathBalls[i].transform.position.z);
			}
		}
		if(transform.parent!=null){
			transform.parent.transform.Rotate(new Vector3(0,0,-Input.GetAxis("Horizontal")*movementSpeed*Time.deltaTime),Space.World);
		}

	}

	void OnCollisionEnter(Collision other){
		if(other.collider.tag=="Planet"){
			cameraScript.LerpToNewPosition(new Vector3(transform.position.x+6f,cameraScript.transform.position.y,cameraScript.transform.position.z));
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
			foreach(GameObject ball in pathBalls){
				ball.SetActive(true);
			}
			// Debug.DrawRay(transform.position, other.contacts[0].normal*10f, Color.green);
		}
	}

	void OnCollisionExit(Collision other){
		if(other.collider.tag=="Planet"){
			hasContactPoint = false;
			Debug.Log("exited");
			rb.useGravity = true;
			transform.parent = null;
			foreach(GameObject ball in pathBalls){
				ball.SetActive(false);
			}
		}
	}
}
