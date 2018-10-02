using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

	private float h;
	private float v;
	private float movementSpeed = 3f;
	private Animator anim;

	private float MouseX_Inp;
	[SerializeField]
	private float MouseX_Sensitivity = 10f;
	private bool onBuilding = false;
	private SpidermanWebSwing swingScript;
	private float totalMouseX = 0f;

	[SerializeField]
	private GameObject hoaxPlayer;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>(); 
		swingScript = GetComponent<SpidermanWebSwing>();
	}
	
	// Update is called once per frame
	void Update () {
		onBuilding = swingScript.onBuilding;
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");
		if(!onBuilding){
			transform.position += hoaxPlayer.transform.TransformDirection(new Vector3(h*movementSpeed*Time.deltaTime,0f,v*movementSpeed*Time.deltaTime));
		}
		else{
			transform.position += hoaxPlayer.transform.TransformDirection(new Vector3(h*movementSpeed*Time.deltaTime,v*movementSpeed*Time.deltaTime,0f));
		}
		anim.SetFloat("h",h);
		anim.SetFloat("v",v);
		if(h!=0f || v!=0f){
			anim.SetBool("isMoving",true);
		}
		else{
			anim.SetBool("isMoving",false);
		}
		BodyXRotate();
	}

	void BodyXRotate(){
		MouseX_Inp = Input.GetAxis("Mouse X") * Time.deltaTime * MouseX_Sensitivity;
		totalMouseX += MouseX_Inp;
		transform.Rotate(new Vector3(0f,MouseX_Inp,0f));
	}

	public void ProperRotation(){
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,totalMouseX,transform.eulerAngles.z);
	}
}
