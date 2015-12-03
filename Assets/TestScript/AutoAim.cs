using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class AutoAim : MonoBehaviour {
	TestSocketIO socket;
	public GameObject bullet;
	bool isplayer = true;
	GameObject aimer;
	GameObject targe;

	public bool shoting = false;
	float shot_total_time=0;
	float shoting_matain = 0.6f;
	
	// Use this for initialization
	void Start () {
		socket = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
		aimer = transform.Find("Aimer").gameObject;
		isplayer = tag=="Player" ? true : false;
	}
	
	// Update is called once per frame
	void Update () {
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
//			transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
			if(isplayer){
			Vector3 targetPostition = new Vector3( targe.transform.position.x, 
			                                      this.transform.position.y, 
			                                      targe.transform.position.z ) ;
				transform.LookAt(targetPostition);
			}
		}

	}

	public void performShoot(){
	

			aimer.transform.LookAt(targe.transform.position);
			if (Physics.Raycast(transform.position, aimer.transform.forward, 10000)) {

				transform.LookAt(targe.transform.position);
				GameObject b =Instantiate(bullet,
				                          new Vector3(transform.position.x + transform.forward.x, transform.position.y + transform.forward.y,transform.position.z + transform.forward.z)
				                          ,transform.rotation) as GameObject;
				b.GetComponent<Rigidbody>().AddForce(new Vector3(b.transform.forward.x*10000,b.transform.forward.y*10000,b.transform.forward.z*10000));
				}

//		else{
//			GameObject b =Instantiate(bullet,
//			                          new Vector3(transform.position.x + transform.forward.x, transform.position.y + transform.forward.y,transform.position.z + transform.forward.z)
//			                          ,transform.rotation) as GameObject;
//			b.GetComponent<Rigidbody>().AddForce(new Vector3(b.transform.forward.x*10000,b.transform.forward.y*10000,b.transform.forward.z*10000));
//		}
	}

	public void setTarge(string id){

		Debug.Log(id+"----");

		foreach(string k in socket.other_players.Keys)
			Debug.Log (k);
		if(!(socket.other_players.TryGetValue(id, out targe)))
			targe = GameObject.FindGameObjectWithTag("Player");
		shoting = true;
		Debug.Log("ss");
	}

	void OnGUI(){
//		if(GUI.RepeatButton(new Rect(Screen.width*(0.9f), Screen.height*0.95f, Screen.width*0.1f, Screen.height*0.05f),"Shot"))
		
	}
}
