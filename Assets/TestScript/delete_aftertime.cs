using UnityEngine;
using System.Collections;

public class delete_aftertime : MonoBehaviour {

	public float life_time = 5;
	float time_pass = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (time_pass > life_time)
			Destroy (gameObject);
		time_pass += Time.deltaTime;
	}
}
