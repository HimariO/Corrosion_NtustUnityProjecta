using UnityEngine;
using System.Collections;

public class CubeOnTouch : MonoBehaviour {
	public GameObject TheCube;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		Input.GetTouch (0)
		if (Input.GetMouseButtonDown(0)) {
			Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
//				if(hit.collider.tag == "Cube")
					Destroy(hit.collider.gameObject);
					
			}
		}
	}
}
