using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReadFileUI : MonoBehaviour {
	string data = "Processing";
	AnalyScreenShot AS;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AnalyScreenShot>();
	}
	
	// Update is called once per frame
	void Update () {
		if(AS.Finished){
			data = "FInish !!";
		}
	}

	void OnGUI(){

		if(GUI.Button(new Rect(Screen.width/2,0,Screen.width/4,Screen.height/16),data)){
//			SaveLoad.Save();
			Application.LoadLevel("GenFromScreen");
		}


	}
}
