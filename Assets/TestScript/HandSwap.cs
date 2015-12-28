using UnityEngine;
using System.Collections;

public class HandSwap : MonoBehaviour {
	[SerializeField]
	float time_delay = 0f;
	float moving_time=0;
	Vector3 startpoint;
	[SerializeField]
	bool moving = false;
	// Use this for initialization
	void Start () {
		startpoint = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		time_delay += Time.deltaTime;

		if(time_delay>70 && !moving){
			moving = true;
			time_delay =0 ;
		}

		if(moving){
//			Debug.Log("loglog");
			transform.Translate(Vector3.left * Time.deltaTime*20);
			moving_time+=Time.deltaTime;

			if(moving_time>10){
				moving=false;
				moving_time=0;
				transform.position = startpoint;
			}
		}

	}
}
