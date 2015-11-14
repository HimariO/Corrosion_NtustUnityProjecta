﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class movment_joystick : MonoBehaviour {
	public float movingspeed;
	public float rushspeed=150;
	public float jump_muti=5;

	public float show_Ver =0;
	public float show_HOr =0;
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

		if (CrossPlatformInputManager.GetAxis ("Vertical") !=0 || CrossPlatformInputManager.GetAxis ("Horizontal") != 0) {
			vector = new Vector3 (transform.forward.x * CrossPlatformInputManager.GetAxis ("Vertical") * movingspeed, 0, transform.forward.z * CrossPlatformInputManager.GetAxis ("Vertical") * movingspeed);
			BODY.AddForce (vector);
			vector = new Vector3 (transform.right.x * CrossPlatformInputManager.GetAxis ("Horizontal") * movingspeed, 0, transform.right.z * CrossPlatformInputManager.GetAxis ("Horizontal") * movingspeed);
			BODY.AddForce (vector);
		}

		if (CrossPlatformInputManager.GetAxis ("Mouse Y") != 0) {
			vector = new Vector3 (
				0,
				transform.up.y * CrossPlatformInputManager.GetAxis ("Mouse Y") * jump_muti,
				0);
			BODY.AddForce (vector);
		}

		transform.Rotate(Vector3.down * (-CrossPlatformInputManager.GetAxis("Mouse X"))*movingspeed/8);
		show_HOr = CrossPlatformInputManager.GetAxis("Horizontal");
		show_Ver =  CrossPlatformInputManager.GetAxis("Vertical ");
	}

	void OnCollisionEnter(Collision collision){
		touch_ground = true;
	}
}