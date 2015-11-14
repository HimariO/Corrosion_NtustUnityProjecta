using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SocketIO;

public class SetURL : MonoBehaviour {
	public InputField input;
	bool showinput = false;
	SocketIO.SocketIOComponent io;
	TestSocketIO test_script;
	string new_url;
	// Use this for initialization
	void Start () {
		io = GameObject.Find("SocketIO").GetComponent<SocketIO.SocketIOComponent>();
		test_script = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(input.isFocused && input.text != "" && Input.GetKey(KeyCode.Return)) {

		}
	}

	void OnOnSubmitMessage(){


	}

	public void eee(string aaa){
		print("submit");
		io.url = input.text;
		io.Awake();
		io.Connect();
		test_script.enabled = true;
		input.text = "";
	}

}
