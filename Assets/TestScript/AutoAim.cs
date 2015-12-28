using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class AutoAim : MonoBehaviour {
	TestSocketIO socket;
	public GameObject bullet;
	GameObject aimer;
	GameObject targe;

	Rigidbody rigidbody;

	bool isplayer = true;

	public float dash_speed = 50;
	public bool shoting = false;
	public bool dashing = false;

	private float shot_total_time=0;
	private float shoting_matain = 0.6f;

	private float dash_total_time = 0f;
	private float dash_matain = 0.5f;


	
	// Use this for initialization
	void Start () {
		socket = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
		aimer = transform.Find("Aimer").gameObject;
		isplayer = tag=="Player" ? true : false;
		this.rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		//dectect and perform regular shooting

		if(shoting){
			performShoot();
			shot_total_time += Time.deltaTime;
		}

		if((CrossPlatformInputManager.GetButton ("Shot")) && !shoting && isplayer){
			foreach (KeyValuePair<string, GameObject> item in socket.other_players) {
				GameObject other= item.Value;
				float angle = Vector3.Angle(transform.forward, other.transform.position-transform.position);

				Vector3 vRota = transform.TransformDirection(Vector3.forward);
				if(angle<20 && angle>-20){
					shoting = true;
					targe = other;
					socket.SendShotting(item.Key);

				}
			}
		}else if(shoting && shot_total_time>shoting_matain){
			shoting = false;
			shot_total_time = 0f;

			if(isplayer){
				Vector3 targetPostition = new Vector3( 
				                                      targe.transform.position.x, 
			                                      		this.transform.position.y, 
			                                      		targe.transform.position.z ) ;
				transform.LookAt(targetPostition);
			}
		}



		//dectect and perform rocket dash
		if(dashing){
			performDash();
			dash_total_time += Time.deltaTime;
		}


		if(CrossPlatformInputManager.GetButton ("Dash") && !dashing && isplayer){
//			Terrain.Explosion(hit, new BlockAir())
			Debug.Log("HI");
			foreach (KeyValuePair<string, GameObject> item in socket.other_players) {
				GameObject other= item.Value;
				float angle = Vector3.Angle(transform.forward, other.transform.position-transform.position);
				
				Vector3 vRota = transform.TransformDirection(Vector3.forward);
				if(angle<20 && angle>-20){
					dashing = true;
					targe = other;
				}
			}
		}else if(dashing && dash_total_time>dash_matain){
			dashing = false;
			dash_total_time = 0f;

			if(isplayer){
				Vector3 targetPostition = new Vector3( targe.transform.position.x, 
				                                      this.transform.position.y, 
				                                      targe.transform.position.z ) ;
				transform.LookAt(targetPostition);
			}
		}
	}
	

	void OnCollisionEnter(Collision collision){
		if(dashing && collision.transform.tag=="Chunk"){
			RaycastHit hit;
			if(Physics.Raycast(transform.position, aimer.transform.forward, out hit))
				Terrain.Explosion(hit, new BlockAir());

		}
	}


	void performShoot(){
			aimer.transform.LookAt(targe.transform.position);
			if (Physics.Raycast(transform.position, aimer.transform.forward, 10000)) {

				transform.LookAt(targe.transform.position);
				GameObject b =Instantiate(bullet,
				                          new Vector3(transform.position.x + transform.forward.x, transform.position.y + transform.forward.y,transform.position.z + transform.forward.z)
				                          , transform.rotation) as GameObject;
				b.GetComponent<Rigidbody>().AddForce(new Vector3(b.transform.forward.x*10000,b.transform.forward.y*10000,b.transform.forward.z*10000));
				}
	}


	void performDash(){
		transform.LookAt(targe.transform.position);
		this.rigidbody.velocity = transform.forward * dash_speed;
	}


	public void setTarge(string id){
		if(!(socket.other_players.TryGetValue(id, out targe)))
			targe = GameObject.FindGameObjectWithTag("Player");
		shoting = true;
	}
	
}
