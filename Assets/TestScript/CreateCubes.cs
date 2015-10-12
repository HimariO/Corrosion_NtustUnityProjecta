using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class CreateCubes : MonoBehaviour {

	public GameObject node_wall;
	public World world;

	public int startX = 0;
	public int startY = 0;
	public int startZ = 0;

	public int sizeX = 10;
	public int sizeZ = 10;

	public int Node_count = 2;
		
	private List<float[]> node_recode= new List<float[]>();
	private int elevation = 1;
	private string[] direction = {"top", "down", "left", "right", "front", "back" };

	float node_size;

	//direction = {"top", "down", "left", "right", "front", "back" };
	private float[] node_wall_x;
	private float[] node_wall_y;
	private float[] node_wall_z;
	private float node_thickness;

	private List<Wall> wall_list = new List<Wall>();

	bool finish = false;
	bool created = false;
	Thread map_t;

	// Use this for initialization
	void Start () {
		node_size = node_wall.GetComponent<Transform>().lossyScale.x;
		node_thickness = node_wall.GetComponent<Transform>().lossyScale.z;
//		path_w_h = path_wall.GetComponent<Transform> ().lossyScale.y;
//		path_length = path_wall.GetComponent<Transform> ().lossyScale.x;
		
		//direction = {"top", "down", "left", "right", "front", "back" };
		node_wall_x = new float[]{0, 0, node_size/2, -node_size/2, 0, 0};
		node_wall_y = new float[]{node_size/2, -node_size/2, 0, 0, 0, 0};
		node_wall_z = new float[]{0, 0, 0, 0, node_size/2, -node_size/2};

		map_t = new Thread (() => generateMap (Node_count));

//		world.CreateChunk (0,0,0);
		Chunk.chunkSize = (int)node_size;

	}
	
	// Update is called once per frame
	void Update () {
//		print (finish + " " + created+ " "+map_t.IsAlive);1

		if (finish && !created) {
			map_t.Abort ();
			map_t.Interrupt ();
			print("end");

			for(int i=0;i<wall_list.Count;i++){
				Vector3 position = wall_list[i].Wall_position;
				Instantiate(node_wall, position, wall_list[i].orintation);
			}
			created=true;
			
		}
	}



	void OnGUI(){

		if (GUI.Button (new Rect (60, 0, 60, 30), "node")) {

//			map_t.Start();
			generateMap(Node_count);

		}
	}


	/**===========================================================**/

	void generateMap(int total){
		if (finish)
			return;

		float[] temp = {0,0,0};
		int offset = Chunk.chunkSize/2;
		Vector3 current_position = new Vector3 (0, 0, 0);

		System.Random r = new System.Random();
//		int face_igone = -2,face_igone_pre = Random.Range(0, 6),face_igone_back=-2;
		int face_igone = -2,face_igone_pre = r.Next()%6,face_igone_back=-2;


		for (int node_cun=0; node_cun<total; node_cun++) {
			Vector3 unvalida_posistion;
			int[] options = {0,1,2,3,4,5};

			int debug = 0;
			do{

//				face_igone = (int)Random.Range (0, 6);

				face_igone = options[(int)r.Next()%options.Length];
				face_igone_back = face_igone_pre%2==0 ? face_igone_pre+1 : face_igone_pre-1;

				unvalida_posistion = new Vector3 (
					current_position.x + node_wall_x [face_igone_pre] * 2,
					current_position.y + node_wall_y [face_igone_pre] * 2,
					current_position.z + node_wall_z [face_igone_pre] * 2
				);
				temp = new float[]{
					current_position.x + node_wall_x [face_igone_pre] * 2,
					current_position.y + node_wall_y [face_igone_pre] * 2,
					current_position.z + node_wall_z [face_igone_pre] * 2
				};
//				print("new point"+node_wall_x [face_igone]+","+node_wall_y [face_igone]+","+node_wall_z [face_igone]);
				if(debug>30 || options.Length<=1){
					print(debug);
					break;
				}

				int[] temp_A = new int[options.Length-1];
				for(int i=0,count=0;i<options.Length;i++){
					if(i!=face_igone){
						temp_A[count]=options[count];count++;
					}
				}
				options = temp_A;

				debug++;

			}while(face_igone==face_igone_back || ListContains(node_recode, temp));
			//

			current_position = unvalida_posistion;
			generateNode(current_position, face_igone, face_igone_back);
			node_recode.Add(temp);
			print("node_recode_count:"+node_recode.Count);
			face_igone_pre = face_igone;


			world.CreateChunk (
				Mathf.FloorToInt(current_position.x),
				Mathf.FloorToInt(current_position.y),
				Mathf.FloorToInt(current_position.z)
				);
			
		}

		finish = true;

	}


	void generateNode(Vector3 position, int face_n, int face_igone_back){


		Vector3 current_position = position;
		int offset = Mathf.FloorToInt(node_size / 2)+(int)node_thickness/2;
		int r = face_n;
		System.Random random = new System.Random();
		if (r == -1)
			r = random.Next () % 6;
//			r = Random.Range (0, 6);

		string dir = direction [r];
		int face_igone = -2;
		face_igone = r;


		print ("face_igone:"+face_igone + " face_igone_back:" + face_igone_back);
		
		for (int face=0; face<6; face++) {
			if (face == face_igone || face == face_igone_back)
				continue;

			float[] wall_offset = {node_thickness,node_thickness,node_thickness};
			wall_offset[0] = node_wall_x[face] == 0 ? 0 : node_thickness/2;
			wall_offset[1] = node_wall_y[face] == 0 ? 0 : node_thickness/2;
			wall_offset[2] = node_wall_z[face] == 0 ? 0 : node_thickness/2;

			wall_offset[0] = node_wall_x[face] > 0 ? wall_offset[0] : -wall_offset[0];
			wall_offset[1] = node_wall_y[face] > 0 ? wall_offset[1] : -wall_offset[1];
			wall_offset[2] = node_wall_z[face] > 0 ? wall_offset[2] : -wall_offset[2];

			Vector3 wall_position = new Vector3 (
				current_position.x + node_wall_x [face] +offset+wall_offset[0]-0.5f,
				current_position.y + node_wall_y [face]+offset+wall_offset[1]-0.5f,
				current_position.z + node_wall_z [face]+offset+wall_offset[2]-0.5f
			);

			
			Quaternion ori;
			
			
			switch (direction [face]) {
			case "top":
				ori = Quaternion.AngleAxis (90, Vector3.right);
				break;
				
			case "down":
				ori = Quaternion.AngleAxis (90, Vector3.right);
				break;
				
			case "left":
				ori = Quaternion.AngleAxis (90, Vector3.down);
				break;
				
			case "right":
				ori = Quaternion.AngleAxis (90, Vector3.down);
				break;
				
			case "front":
				ori = Quaternion.AngleAxis (90, Vector3.forward);
				break;
				
			case "back":
				ori = Quaternion.AngleAxis (90, Vector3.forward);
				break;
				
			default:
				continue;
			}

			wall_list.Add(new Wall(ori, wall_position, node_wall));
//			Instantiate (node_wall, wall_position, ori);
//			StartCoroutine(Wait());

		}

	}




	IEnumerator Wait() {
		print(Time.time);
		yield return new WaitForSeconds(1);
		print(Time.time);
	}

	bool ListContains(List<float[]> floatlist, float[] compare){
		float[] a;
		for(int i=0;i<floatlist.Count;i++){
			a = floatlist[i];
			if(Mathf.Approximately(a[1], compare[1]) 
			   && Mathf.Approximately(a[0], compare[0]) 
			   && Mathf.Approximately(a[2], compare[2]))
				return true;
		}
		return false;
	}

	

}
