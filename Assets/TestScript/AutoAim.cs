using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class AutoAim : MonoBehaviour {
	TestSocketIO socket;
	public GameObject bullet;

	GameObject aimer;
	GameObject targe;

	bool shoting = false;
	
	// Use this for initialization
	void Start () {
		socket = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
		aimer = GameObject.Find("Aimer");
	}
	
	// Update is called once per frame
	void Update () {
		if((CrossPlatformInputManager.GetButton ("Shot"))){
			foreach (KeyValuePair<string, GameObject> item in socket.other_players) {
				GameObject other= item.Value;
				float angle = Vector3.Angle(transform.forward, other.transform.position-transform.position);

				Vector3 vRota = transform.TransformDirection(Vector3.forward);
				if(angle<20 && angle>-20){
					shoting = true;
					targe = other;
					aimer.transform.LookAt(other.transform.position);

					if (Physics.Raycast(transform.position, aimer.transform.forward, 10000)) {
						transform.LookAt(other.transform.position);
						GameObject b =Instantiate(bullet,
						            new Vector3(transform.position.x + transform.forward.x, transform.position.y + transform.forward.y,transform.position.z + transform.forward.z)
						            ,transform.rotation) as GameObject;
						b.GetComponent<Rigidbody>().AddForce(new Vector3(b.transform.forward.x*10000,b.transform.forward.y*10000,b.transform.forward.z*10000));
					}
				}
			}
		}else if(shoting){
			shoting = false;
//			transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
			Vector3 targetPostition = new Vector3( targe.transform.position.x, 
			                                      this.transform.position.y, 
			                                      targe.transform.position.z ) ;
			transform.LookAt(targetPostition);
		}
	}

	void OnGUI(){
//		if(GUI.RepeatButton(new Rect(Screen.width*(0.9f), Screen.height*0.95f, Screen.width*0.1f, Screen.height*0.05f),"Shot"))
		
	}
}
