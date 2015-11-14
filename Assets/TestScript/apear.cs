using UnityEngine;
using System.Collections;

public class apear : MonoBehaviour {
	float size = 1;
	float stackup = 0;
	// Use this for initialization
	void Start () {
		size = transform.localScale.x;
		transform.localScale = new Vector3(0,0,0);
			
	}
	
	// Update is called once per frame
	void Update () {
		if (stackup < size && size>stackup + size * Time.deltaTime * 3) {
			stackup += size * Time.deltaTime * 3;
		} else {
			stackup = size;
		}
		transform.localScale = new Vector3 (
			stackup,
			stackup,
			stackup
			);
	}
}
