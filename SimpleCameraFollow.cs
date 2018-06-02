using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour {

	[SerializeField]
	private GameObject target;
	private Vector3 offset;

	// Use this for initialization
	void Awake () {
		offset = transform.position - target.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.position = target.transform.position + offset;
	}
}
