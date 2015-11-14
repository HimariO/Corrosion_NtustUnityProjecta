using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

public class Modify : MonoBehaviour
{

    Vector2 rot;
	public GameObject Explosion_Block;
	public GameObject[] Spec_Block;
	TestSocketIO socket;
	World world;

	void Start(){
		socket = GameObject.Find("ConnectServer").GetComponent<TestSocketIO>();
		world = GameObject.Find("WorldStartHere").GetComponent<World>();
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.E) || CrossPlatformInputManager.GetButton ("Mod"))
//		if(Input.GetAxis ("Fire1")!=0)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
            {
				ReplaceBlocks(Terrain.Explosion(hit, new BlockAir()));
            }
        }
		
    }

	public void ReplaceBlocks(List<Vector3> prefab_pos){
		int force = 50;

		if (prefab_pos!= null){
			for(int i=0;i<prefab_pos.Count;i++){
				GameObject temp;
				int r= Random.Range(0,Spec_Block.Length*3);
				if(r>=Spec_Block.Length){
					temp = Instantiate(Explosion_Block, prefab_pos[i], Explosion_Block.transform.rotation) as GameObject;
					socket.SendModifyMap(prefab_pos[i],"BlockMap");
				}
				else{
					temp = Instantiate(Spec_Block[r], prefab_pos[i], Spec_Block[r].transform.rotation) as GameObject;
					socket.SendModifyMap(prefab_pos[i], "BlockSpecial_"+r);
				}
				
				temp.GetComponent<Rigidbody>().AddForce(
					new Vector3(
					transform.forward.x*force,
					transform.forward.y*force,
					transform.forward.z*force
					));
			}
		}
	}

	public void ReplaceBlock(Vector3 pos,int r=-1){
		int force = 50;
		world.SetBlock((int)pos.x, (int)pos.y,(int)pos.z, new BlockAir());
				GameObject temp;

				if(r==-1){
					temp = Instantiate(Explosion_Block, pos, Explosion_Block.transform.rotation) as GameObject;
				}
				else{
					temp = Instantiate(Spec_Block[r], pos, Spec_Block[r].transform.rotation) as GameObject;
				}
				
				temp.GetComponent<Rigidbody>().AddForce(
					new Vector3(
					transform.forward.x*force,
					transform.forward.y*force,
					transform.forward.z*force
					));
			

	}
}