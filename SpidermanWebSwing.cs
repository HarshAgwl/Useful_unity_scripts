using System.Collections;
using System.Collections.Generic;
using UnityEngine;
		
public class SpidermanWebSwing : MonoBehaviour {

	//Component References
	private Rigidbody rb;
	private Animator anim;

	//Swing Line Variables
	[SerializeField]
	private LineRenderer webLine;
	[SerializeField]
	private GameObject swingEndPoint;
	[SerializeField]
	private GameObject pointOfRotation;
	[SerializeField]
	private float initialVelocity = 20f;
	[SerializeField]
	private GameObject velocityHelper;
	
	//State Variables
	public bool isSwinging = false;
	private bool stop = false;
	private bool inAir = false;
	[HideInInspector]
	public bool onBuilding = false;
	private bool run = false;
	private bool goingToBuilding = false;
	[SerializeField]
	private bool hasStartedOnBuilding = false;
	
	[SerializeField]
	private GameObject downRaycast;
	private RaycastHit hit;
	private RaycastHit hit2;

	[SerializeField]
	private Camera camera;

	private Ray ray;

	// Use this for initialization
	void Awake () {
		rb = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		DrawSpidermanSwing();
		Jump();
		Swing();
		GoToBuilding();
		DownRaycast();
		Run();
		GravityRequirement();
	}

	void GravityRequirement(){
		if(isSwinging || onBuilding || goingToBuilding){
			rb.useGravity = false;
		}
		else{
			rb.useGravity = true;
		}
	}

	void FixedUpdate(){
		if(Vector3.Angle(transform.forward,-(pointOfRotation.transform.position-transform.position))<=2.5f){
			stop = true;
			isSwinging = false;
		}

		if(stop==false && isSwinging){
			rb.AddForce((pointOfRotation.transform.position-transform.position).normalized*rb.mass*initialVelocity*initialVelocity/(pointOfRotation.transform.position-transform.position).magnitude);
		}
		else{
		}
	}

	void GoToBuilding(){
		ray = camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
		if(Input.GetMouseButtonDown(0)){
			if(Physics.Raycast(ray,out hit,Mathf.Infinity)){
				Debug.Log(hit.point);
				Debug.Log(hit.collider.gameObject.name);
				rb.velocity = ((hit.point - transform.position).normalized)*50f;
				goingToBuilding = true;
			}
		}
	}

	void Run(){
		if(Input.GetKey(KeyCode.LeftShift)){
			run = true;
			anim.SetBool("isRunning",run);
		}
		else{
			run = false;
			anim.SetBool("isRunning",run);
		}
	}

	void DownRaycast(){
		Debug.DrawRay(downRaycast.transform.position,-transform.up*1.5f,Color.green);
		if(Physics.Raycast(downRaycast.transform.position,-transform.up*1.5f,out hit,1.5f)){
			inAir = false;
			anim.SetBool("inAir",inAir);
			if(hit.collider.tag=="Ground" || hit.collider.tag=="Building"){
				// transform.up = hit.normal;
			}
			Debug.Log(hit.collider.tag);
			// if(hit.collider.tag=="Building"){
			// 	hasStartedOnBuilding = true;
			// }
			// else if(hit.collider.tag!="Building" && hasStartedOnBuilding){
			// 	hasStartedOnBuilding = false;
			// 	StartCoroutine(ExitFromBuildingState(true));
			// }

		}
		else{
			inAir = true;
			anim.SetBool("inAir",inAir);
		} 

		if(Physics.Raycast(downRaycast.transform.position + new Vector3(0f,1f,0f),transform.forward*1.5f,out hit,1.5f))
		{
			if(hit.collider.tag=="Building"){
				hasStartedOnBuilding = true;
			}
			else if(hit.collider.tag!="Building" && hasStartedOnBuilding){
				hasStartedOnBuilding = false;
				StartCoroutine(ExitFromBuildingState(true));
			}
		}


		// Debug.Log(hit.collider.tag);
		// if(onBuilding && hit.collider==null){
		// 	if(hit.collider.tag!="Building"){
		// 		StartCoroutine(ExitFromBuildingState(true));
		// 	}
		// }
		
	}

	// IEnumerator StartChecking(){
	// 	yield return new WaitForSeconds(0.25f);
	// 	if(onBuilding){
	// 		if(Physics.Raycast(downRaycast.transform.position,-transform.up*1.5f,out hit,0.6f)){
	// 			//Do Nothing
	// 		}
	// 		else{
	// 			 ExitFromBuildingState(true);
	// 		}
	// 	}
	// }

	void DrawSpidermanSwing(){
		if(!isSwinging){
			webLine.enabled = false;
		}
		else{
			webLine.enabled = true;
			webLine.SetPosition(0,pointOfRotation.transform.position);
			webLine.SetPosition(1,swingEndPoint.transform.position);
		}
	}

	void Jump(){
		if(Input.GetAxis("Vertical")!=0f && Input.GetKeyDown(KeyCode.Space)){
			Debug.Log("Hello");
			rb.AddForce((transform.forward + transform.up) * 20f, ForceMode.Impulse);
		}
	}

	void Swing(){
		anim.SetBool("isSwinging",isSwinging);
		if(Input.GetKeyDown(KeyCode.Z)){
			if(!isSwinging){
				stop = false;
				pointOfRotation.transform.position = transform.position + transform.forward*8f + transform.up*8f;
				velocityHelper.transform.rotation = Quaternion.LookRotation(pointOfRotation.transform.position-transform.position);
				rb.velocity = -velocityHelper.transform.up * initialVelocity;
				isSwinging = true;
			}
			else{
				stop = true;
				isSwinging = false;	
			}
		}
	}

	void OnCollisionEnter(Collision other){
		if(other.collider.tag=="Building" && inAir){
			// transform.eulerAngles = new Vector3(transform.eulerAngles.x-90f,transform.eulerAngles.y,transform.eulerAngles.z);
			onBuilding = true;
			// transform.forward = -hit.normal;
			anim.SetBool("onBuilding",true);
			goingToBuilding = false;
			rb.velocity = new Vector3(0f,0f,0f);
		}
		if(other.collider.tag=="Ground" && onBuilding){
			StartCoroutine(ExitFromBuildingState());
		}
	}

	// void OnCollisionExit(Collision other){
	// 	if(other.collider.tag=="Building"){
	// 		StartCoroutine(ExitFromBuildingState(true));
	// 	}
	// }

	IEnumerator ExitFromBuildingState(bool addForce=false){
		// transform.eulerAngles = new Vector3(transform.eulerAngles.x+90f,transform.eulerAngles.y,transform.eulerAngles.z);
		onBuilding = false;
		anim.SetBool("onBuilding",false);
		if(addForce){
			rb.AddForce(Vector3.up*12.5f,ForceMode.Impulse);
			yield return new WaitForSeconds(0.1f);
			rb.AddForce(transform.forward*2f,ForceMode.Impulse);
		}
	}
}
