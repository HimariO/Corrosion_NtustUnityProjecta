using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	public int PlayerHealth = 200;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckOutOfBound();
	}

	void CheckOutOfBound (){
		if(transform.position.y<-20){
			transform.position = new Vector3(10f, 10f, 10f);
		}
	}

	void OnCollisionEnter(Collision collision){
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log("ssse");
	}


}
