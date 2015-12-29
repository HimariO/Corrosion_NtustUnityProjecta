﻿using UnityEngine;
using System.Collections;

public class SpecCubeOnColl : MonoBehaviour {
	World world;
	TestSocketIO socket;
	public BlockSpecial.Specal_type type;
	// Use this for initialization
	void Start () {
		world = GameObject.Find("WorldStartHere").GetComponent<World>();
		socket = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "Player" ){
			collision.gameObject.GetComponent<PlayerStatus>().GetAitem(type);
		}
	}
}
