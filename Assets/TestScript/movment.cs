﻿using UnityEngine;
using System.Collections;

public class movment : MonoBehaviour {
	public float movingspeed;
	public float rushspeed=150;
	public float jump_muti=5;
	private Vector3 vector;
	private Rigidbody BODY;
	private GameObject rb;
	bool touch_ground = true;

	// Use this for initialization
	void Start () {
		BODY = GetComponent<Rigidbody>();


	}
	
	// Update is called once per frame Vertical
	void Update () {

		if (Input.GetAxis ("Vertical") != 0) {
			vector = new Vector3 (transform.forward.x * Input.GetAxis ("Vertical") * movingspeed, BODY.velocity.y, transform.forward.z * Input.GetAxis ("Vertical") * movingspeed);
			BODY.AddForce (vector);
		}
		//		vector = new Vector3 (transform.forward.x*Input.GetAxis("Vertical")*movingspeed, Input.GetAxisRaw("Jump")*movingspeed , transform.forward.z*Input.GetAxis("Vertical")*movingspeed);



//		if (touch_ground && Input.GetAxisRaw ("Jump")!=0) {
//			BODY.AddForce (new Vector3 (transform.forward.x*movingspeed*rushspeed , 10 * jump_muti, transform.forward.z*movingspeed*rushspeed));
//			touch_ground = false;
//		}

		transform.Rotate(Vector3.down * (-Input.GetAxis("Horizontal"))*movingspeed/5);



	}

	void OnCollisionEnter(Collision collision){
		touch_ground = true;
	}
}