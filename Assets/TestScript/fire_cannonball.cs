using UnityEngine;
using System.Collections;

public class fire_cannonball : MonoBehaviour {
	float time = 0;
	float time_to_repeat = 2;
	int speed = 1000;
	public GameObject ball;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if(time>time_to_repeat){
			time=0;

			GameObject instance = Instantiate (ball);
			instance.transform.position = transform.position;
			instance.GetComponent<Rigidbody> ().AddForce (new Vector3 (
				transform.forward.x*speed,0,transform.forward.z*speed
			));
		}
	}
}
