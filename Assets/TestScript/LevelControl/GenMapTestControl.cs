using UnityEngine;
using System.Collections;

public class GenMapTestControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<AnalyScreenShot>().enabled=true;
		GetComponent<AnalyScreenShot>().StartProcess();
		GetComponent<MapFromScreenshot>().LoadAndGenTest();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
