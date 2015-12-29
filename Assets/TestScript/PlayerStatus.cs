using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	public int PlayerHealth = 200;
	private bool isplayer = false;
	// Use this for initialization
	void Start () {
		isplayer = tag=="Player" ? true : false ;
	}
	
	// Update is called once per frame
	void Update () {
		CheckOutOfBound();
		CheckAlive();

	}

	void CheckOutOfBound (){
		if(transform.position.y<-20 || transform.position.y>50){
			transform.position = new Vector3(10f, 10f, 10f);
		}
	}

	void CheckAlive(){
		if(PlayerHealth<=0){
			transform.position = new Vector3(10f, 10f, 10f);
			PlayerHealth  =200;
		}
	}

	void OnCollisionEnter(Collision collision){
	}

	void OnTriggerEnter(Collider other) {
		if(other.tag == "Bullet"&& isplayer){
			PlayerHealth -=5;
		}
	}


}
