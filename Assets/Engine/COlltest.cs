using UnityEngine;
using System.Collections;

public class COlltest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		float scale = Time.deltaTime*5f+1;
		transform.localScale = new Vector3(transform.localScale.x*scale, transform.localScale.y*scale, transform.localScale.z*scale);

		if(transform.localScale.x>20)
			Destroy(gameObject);
	}

	void OnCollisionStay(Collision collision) {
		Debug.Log("stay");
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal, Color.white);
			Debug.Log(collision.transform.position.x+"ss");
		}

		
	}
}


