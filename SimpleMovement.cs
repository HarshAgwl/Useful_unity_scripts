using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour {

	private float h;
	private float v;
	[SerializeField]
	private float moveSpeed=2f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");
		transform.position += new Vector3(h*moveSpeed,0f,v*moveSpeed)*Time.deltaTime ;
	}
}
