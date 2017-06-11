using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ThirdPersonController : MonoBehaviour {

	float h;
	float v;
	public float runfactor =2.5f;
	Vector3 movement;

	Animator anim;

	int _currentweapon;

	public GameObject[] Weapons;
	int _currentIndex=0;
	float changeTime=2f;
	public float Time_track;



	public GameObject _img;
	Image img;



	int currentweapon{
		get{
			return _currentIndex;
		}
		set{

			print(_currentIndex);

			Time_track=Time.time+changeTime;

			//Make the current weapon in invisible

			Weapons[_currentIndex].SetActive(false);
			_currentIndex = value;

			//Make the newly selected weapon visible
			Weapons[_currentIndex].SetActive(true);

			print(_currentIndex);

			WeaponInfo info;

			info=Weapons[_currentIndex].GetComponent<WeaponInfo>();

			img.sprite = info.obj;

			anim.SetFloat("w_type",info.anim_index);


		}
	}

	void Awake(){
		img = _img.GetComponent<Image>();
	}

	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {

		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");

		if(Input.GetKey(KeyCode.LeftShift)){
			anim.SetFloat("m_speed",1f);
			h*=runfactor;
			v*=runfactor;
		}
		else{
			anim.SetFloat("m_speed",0f);
		}

		movement = new Vector3(h*Time.deltaTime,0f,v*Time.deltaTime);


		transform.rotation = Quaternion.LookRotation(movement);


		//Change Weapon
		if(Time.time>Time_track){

			if(Input.GetAxis("Mouse ScrollWheel") > 0){
				if(_currentIndex==(Weapons.Length-1)){
				 currentweapon = 0;
				}
				else{
				 currentweapon = _currentIndex+1;
				}
			}
			
			if(Input.GetAxis("Mouse ScrollWheel") < 0){
				if(_currentIndex!=0){
				 currentweapon = _currentIndex-1;
				}
				else{
				 currentweapon = (Weapons.Length-1);
				}
			}
		}

		if(Input.GetButton("Fire1")){
			anim.SetBool("shoot",true);
		}
		else{
			anim.SetBool("shoot",false);
		}





		transform.position += movement;
		AnimateMovement(h,v);
		
	}

	void AnimateMovement(float x1,float y1)
	{

		anim.SetFloat("x_inp",x1);
		anim.SetFloat("y_inp",y1);



		if(h!=0 || v!=0){
			anim.SetBool("is_moving",true);
		}
		else{
			anim.SetBool("is_moving",false);
		}


	}
}
