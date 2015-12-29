using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerStatus : MonoBehaviour {
	public int PlayerHealth = 200;
	private bool isplayer = false;

	Image SuckIMG, BumpIMG;
	World world;
	TestSocketIO socket;

	public int Suck_item = 0;
	public int Bump_item = 0;

	// Use this for initialization
	void Start () {
		isplayer = tag=="Player" ? true : false ;
		SuckIMG= GameObject.Find("ItemSuck").GetComponent<Image>();
		BumpIMG = GameObject.Find("ItemBump").GetComponent<Image>();
		world = GameObject.Find("WorldStartHere").GetComponent<World>();
		socket = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckOutOfBound();
		CheckAlive();

		if(Suck_item>0){
			if(!SuckIMG.enabled){
				SuckIMG.enabled = true;
			}
			if(CrossPlatformInputManager.GetButton("ItemSuck")){
				WorldPos WS = new WorldPos((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
				
				world.SpecialBlockEff(WS, BlockSpecial.Specal_type.Suck, BlockSpecial.Owner.me);
				socket.SendSetSpecBlock(WS, BlockSpecial.Specal_type.Suck);
				Suck_item--;
			}
		}else if(SuckIMG.enabled && Suck_item<=0){SuckIMG.enabled = false;}


		if(Bump_item>0){
			if(!BumpIMG.enabled){
				BumpIMG.enabled = true;
			}
			if(CrossPlatformInputManager.GetButton("ItemBump")){
				WorldPos WS = new WorldPos((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
				
				world.SpecialBlockEff(WS, BlockSpecial.Specal_type.Bound, BlockSpecial.Owner.me);
				socket.SendSetSpecBlock(WS, BlockSpecial.Specal_type.Bound);
				Bump_item--;
			}
		}else if(BumpIMG.enabled && Bump_item<=0){BumpIMG.enabled = false;}
	}


	void OnGUI(){

	}


	void CheckOutOfBound (){
		if(transform.position.y<-20 || transform.position.y>70){
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

	public void GetAitem(BlockSpecial.Specal_type type){
		switch(type){
		case BlockSpecial.Specal_type.Bound:
			Bump_item++;
			break;
		case BlockSpecial.Specal_type.Suck:
			Suck_item++;
			break;
		}
	}


}
