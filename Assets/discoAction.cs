using UnityEngine;
using System.Collections;

public class discoAction : MonoBehaviour {

	Material mt;
	float time_req=1f;
	float time_curr =0f;
	bool bye = false;

	float ani_time=0f;

	// Use this for initialization
	void Start () {
		Renderer rend = GetComponent<Renderer>();
		mt = rend.material;
	}
	
	// Update is called once per frame
	void Update () {
		ani_time+=Time.deltaTime;
		

		if((time_curr<time_req && bye)|| ani_time>13){
			time_curr+=Time.deltaTime;
			mt.SetFloat("_SliceAmount", (time_curr/time_req)*0.6f);
		}else if(time_curr>=time_req){
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.tag == "Player" ){
			bye = true;
		}
	}
}