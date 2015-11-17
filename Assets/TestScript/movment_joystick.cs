using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class movment_joystick : MonoBehaviour {
	public float movingspeed;
	public float rushspeed=150;
	public float jump_muti=5;

	public float speed_cap = 50f;
	public float fly_time_lime = 5f;

	[SerializeField]
	private float show_Ver =0;
	[SerializeField]
	private float show_HOr =0;
	

	[SerializeField]
	private Vector3 speed;
	private Vector3 vector;
	private Rigidbody BODY;
	private GameObject rb;
	[SerializeField]
	private float fly_total_time=0f;
	

	bool touch_ground = true;
	bool flying = false;


	// Use this for initialization
	void Start () {
		BODY = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame Vertical
	void Update () {
		speed = BODY.velocity;
		if (CrossPlatformInputManager.GetAxis ("Vertical") !=0 || CrossPlatformInputManager.GetAxis ("Horizontal") != 0) {
			if(speed.x < speed_cap && speed.z < speed_cap){
				vector = new Vector3 (transform.forward.x * CrossPlatformInputManager.GetAxis ("Vertical") * movingspeed, 0, transform.forward.z * CrossPlatformInputManager.GetAxis ("Vertical") * movingspeed);
				BODY.AddForce (vector);
				vector = new Vector3 (transform.right.x * CrossPlatformInputManager.GetAxis ("Horizontal") * movingspeed, 0, transform.right.z * CrossPlatformInputManager.GetAxis ("Horizontal") * movingspeed);
				BODY.AddForce (vector);
			}
		}


		if (CrossPlatformInputManager.GetAxis ("Mouse Y") != 0) {
//			flying = true;
			if(fly_total_time<fly_time_lime){
				if(speed.y < speed_cap ){
				vector = new Vector3 (0, transform.up.y * CrossPlatformInputManager.GetAxis ("Mouse Y") * jump_muti, 0);
				BODY.AddForce (vector);
				fly_total_time += Time.deltaTime;
				}
			}
		}
		else{
			if(fly_total_time>0 && !flying)
				fly_total_time -= Time.deltaTime*2;
			else if(fly_total_time<0 && !flying){
				fly_total_time = 0f;
			}
		}


		transform.Rotate(Vector3.down * (-CrossPlatformInputManager.GetAxis("Mouse X"))*movingspeed/8);
		show_HOr = CrossPlatformInputManager.GetAxis("Horizontal");
		show_Ver =  CrossPlatformInputManager.GetAxis("Vertical ");
	}

	void OnCollisionEnter(Collision collision){
		touch_ground = true;
	}
}
