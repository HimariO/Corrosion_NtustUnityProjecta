using UnityEngine;
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
		

		foreach(ContactPoint cp in collision.contacts){
			Block block = world.GetBlock((int)cp.point.x, (int)cp.point.y, (int)cp.point.z);
			Debug.Log(block.GetType());

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
		if(sucked_timer == SuckedTime)
			sucked_timer=0;sucked=false;
	}
}
