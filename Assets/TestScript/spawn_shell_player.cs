using UnityEngine;
using System.Collections;

public class spawn_shell_player : MonoBehaviour {
	public GameObject Shell;
	float time_pass = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		time_pass += Time.deltaTime;
		if (Input.GetAxis ("Fire1")!=0 && time_pass>1) {
			print("Fire1");
			time_pass = 0;
			Vector3 spawn_position = new Vector3(
				transform.position.x+transform.forward.x*5,
				transform.position.y,
				transform.position.z+transform.forward.z*5
				);
			Instantiate(Shell, spawn_position, transform.rotation);

		}
	}
}
