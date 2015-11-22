using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scroll : MonoBehaviour {
	Scrollbar sb;
	// Use this for initialization
	void Start () {
		sb =GetComponent<Scrollbar>();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(sb.value);
	}
}
