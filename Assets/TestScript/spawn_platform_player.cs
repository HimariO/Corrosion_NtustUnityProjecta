using UnityEngine;
using System.Collections;

public class spawn_platform_player : MonoBehaviour {

	public GameObject Platform;
	float time_pass = 0;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time_pass += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.R) && time_pass>1) {

			time_pass = 0;
			Vector3 spawn_position = new Vector3(
				transform.position.x+transform.forward.x*2,
				transform.position.y-1,
				transform.position.z+transform.forward.z*2
				);
			Instantiate(Platform, spawn_position, transform.rotation);
			
		}
		
	}
}
