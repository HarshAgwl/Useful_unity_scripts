using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarController : MonoBehaviour {

	[SerializeField]
	private Transform[] tyreTransforms;
	[SerializeField]
	private float forceByEachTyre = 25f;
	[SerializeField]
	private float tyreRaycastDistance = 0.5f;
	[SerializeField]
	private float steerSpeed = 20f;

	
	private float maxSpeed  = 200f;
	[SerializeField]
	private float noNitroMaxSpeed;
	[SerializeField]
	private float nitroMaxSpeed;
	
	//Full Nitro when nitroAmount = 100f
	[SerializeField]
	private float nitroAmount = 100f;
	
	private RaycastHit hit;
	private Rigidbody rb;

	private Vector3 xzVelocity = Vector3.zero;

	[SerializeField]
	private float brakeFactor = 2f;

	enum Direction {X,Y,Z};

	[SerializeField]
	private Direction fakeTyreRotateAxis = Direction.X;

	[SerializeField]
	private ParticleSystem[] nitroParticleSystems;

	void Awake () {
		rb = GetComponent<Rigidbody>();
		foreach(ParticleSystem psytem in nitroParticleSystems){
			if(psytem.isPlaying){
				psytem.Stop();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		// Debug.Log(CalculateTyreFactor());		
		LimitMaxSpeed();
		SteerCar();
		
	}

	void FixedUpdate(){
		Nitro();
		Accelerate();
		Brake();
	}

	void Accelerate(){
		rb.AddForce(transform.forward*CalculateTyreFactor()*forceByEachTyre*Input.GetAxis("Vertical"));
	}

	void Nitro(){
		if(Input.GetKey(KeyCode.N) && nitroAmount>=0.1f){
			foreach(ParticleSystem psytem in nitroParticleSystems){
				if(!psytem.isPlaying){
					psytem.Play();
				}
			}
			nitroAmount -= 10f*Time.deltaTime;
			//Extra force being provided to car when nitro is beieng used
			rb.AddForce(transform.forward*CalculateTyreFactor()*forceByEachTyre*Input.GetAxis("Vertical"));
			maxSpeed = nitroMaxSpeed;
			
		}
		else{
			foreach(ParticleSystem psytem in nitroParticleSystems){
				if(psytem.isPlaying){
					psytem.Stop();
				}
			}
			maxSpeed = noNitroMaxSpeed;
		}
	}

	float CalculateTyreFactor(){
		float i=0;
		foreach(Transform tyreTransform in tyreTransforms){
			if(Physics.Raycast(tyreTransform.position,-Vector3.up,out hit,tyreRaycastDistance)){
				Debug.Log(hit.collider.name);
				if(hit.collider.tag=="RaceTrack"){
					i++;
				}
				else {
					i += 0.5f;
				}
			}
		}
		Debug.Log(i);
		return i;
	}

	void LimitMaxSpeed(){
		xzVelocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
		if(xzVelocity.magnitude>maxSpeed){
			xzVelocity = xzVelocity.normalized * maxSpeed;
			rb.velocity = new Vector3(xzVelocity.x,rb.velocity.y,xzVelocity.z);
		}
	}

	void SteerCar(){
		Vector3 temp;
		float factor = 0f;
		// if(Input.GetAxis("Vertical")>0.1f || Input.GetAxis("Vertical")<-0.1f){
		if(rb.velocity.sqrMagnitude>=1f && Mathf.Abs(Input.GetAxis("Horizontal"))>=0.1f && CalculateTyreFactor()>0){
			transform.Rotate(Vector3.up*Input.GetAxis("Horizontal")*Time.deltaTime*steerSpeed,Space.World);

			factor = Mathf.Round(Input.GetAxis("Vertical"));
			if(factor==0f){
				if(Vector3.Angle(-transform.forward,rb.velocity)<2f){
					factor = -1f;
				}
				else{
					factor = 1f;
				}
				
			}
			temp = transform.forward*(new Vector3(rb.velocity.x,0f,rb.velocity.z)).magnitude*factor;
			rb.velocity = new Vector3(temp.x,rb.velocity.y,temp.z);
			// rb.AddTorque(transform.right*1000f*Input.GetAxis("Horizontal"));
			//Very Important Line(s)
			
			// rb.velocity = transform.forward*Input.GetAxis("Vertical")*rb.velocity.magnitude;
			
			//-------------------

			// foreach(Transform tyreTransform in tyreTransforms){
			// 	tyreTransform.Rotate(new Vector3(0f,Input.GetAxis("Vertical")*1000f),Space.Self);
			// }
		}

		foreach(Transform tyreTransform in tyreTransforms){
			if(fakeTyreRotateAxis==Direction.X){
				tyreTransform.Rotate(new Vector3(Input.GetAxis("Vertical")*1000f,0f,0f),Space.Self);
			}
			else if(fakeTyreRotateAxis==Direction.Y){
				tyreTransform.Rotate(new Vector3(0f,Input.GetAxis("Vertical")*1000f,0f),Space.Self);
			}
			else{
				tyreTransform.Rotate(new Vector3(0f,0f,Input.GetAxis("Vertical")*1000f),Space.Self);
			}
		}
	}

	public float ReturnPlayerMaxSpeed(){
		return noNitroMaxSpeed;
	}

	void Brake(){
		if(Input.GetKey(KeyCode.B)){
			rb.AddForce(CalculateTyreFactor()*forceByEachTyre*rb.velocity.normalized*-1*brakeFactor);
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag=="Pickup"){
			other.gameObject.SetActive(false);
		}
	}
}
