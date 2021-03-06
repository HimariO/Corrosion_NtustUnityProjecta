﻿using UnityEngine;
using System.Collections;

public class PlayerTouchspecBlock : MonoBehaviour {
	World world;
	Rigidbody rigi;
	public float SuckedTime = 2;
	public float BoundForce = 80f;

	bool sucked = false;
	Vector3 sucked_pos;
	float sucked_timer =0 ;
	// Use this for initialization
	void Start () {
		world = GameObject.Find("WorldStartHere").GetComponent<World>();
		rigi = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(sucked)
			Suck();
		
	}

	void OnCollisionEnter(Collision collision){
		if(collision.transform.tag=="Mine")
			Teleport();


		foreach(ContactPoint cp in collision.contacts){
			Block block = world.GetBlock((int)cp.point.x, (int)cp.point.y, (int)cp.point.z);

			if(block is BlockSpecial){
				BlockSpecial t = (BlockSpecial)block;

				if(t.owner == BlockSpecial.Owner.other)
					switch(t.type){
					case BlockSpecial.Specal_type.Bound:
						Bound();
						break;

					case BlockSpecial.Specal_type.Suck:
						sucked=true;
						sucked_pos = transform.position;
						break;
					}
			}
		}
	}

	void Bound(){
		rigi.velocity=(new Vector3(0f, BoundForce, 0f));
	}

	void Suck(){
		sucked_timer += Time.deltaTime;
		transform.position = sucked_pos;
		if(sucked_timer == SuckedTime){
			sucked_timer=0;sucked=false;
		}
	}

	void Teleport(){
		while(true){
			float r_x = Random.Range(-50, 50);
			float r_y = Random.Range(-50, 50);
			float r_z = Random.Range(-50, 50);

			if(!(world.GetBlock((int)(transform.position.x+r_x), (int)(transform.position.y+r_y), (int)(transform.position.z+r_z)) is BlockAir));{
				transform.position = new Vector3(transform.position.x+r_x,transform.position.y+r_y,transform.position.z+r_z);
				break;
			}
		}

	}
}
