using UnityEngine;
using System.Collections;

public class delete_aftercrush : MonoBehaviour {
	public float deletetime = 2;
	float time_pass = 0;
	bool start_delete = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (start_delete) {
			if (time_pass > deletetime)
				Destroy (gameObject);
			time_pass += Time.deltaTime;
		}
	}

	void OnCollisionEnter (Collision col)
	{
		start_delete = true;
//		if(col.gameObject.name == "prop_powerCube")
//		{
//			Destroy(col.gameObject);
//		}
	}

	void OnTriggerEnter(Collider other) {
		start_delete = true;
	}
}
