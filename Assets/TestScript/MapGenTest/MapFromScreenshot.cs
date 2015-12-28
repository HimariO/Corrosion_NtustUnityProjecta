using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapFromScreenshot : MonoBehaviour {
	public GameObject sCube;
	public World world;
	AnalyScreenShot analy; 

	public int start_x=0 ,start_y=0 ,start_z=0;

	int chunk_size = 16;
	int[,,] colorMap;
	public int[,] cubMap ;
	List<List<int[]>> edge_group;
	//[group, index, x or y]

	// Use this for initialization
	void Start() {
		GameObject temp;
		world = GetComponent<World>();
//		analy = GetComponent<AnalyScreenShot>();
		LoadDataFromF();

		GenMapBase();
//		GenFloatingIsand();
	}
	
	// Update is called once per frame
	void Update () {

	}


	public void LoadDataFromF(){
		colorMap = SaveLoad.LoadColorMap();
//		edge_group = SaveLoad.LoadEdgeGroup();
		cubMap = SaveLoad.LoadEdgeMap();

	}

	public void GenMapBase(){
//		colorMap = analy.r_color_map;
//		cubMap = analy.r_edge_map;


		for(int y=start_y; y<start_y+50 ; y+=chunk_size){
			for(int z=start_z; z<start_z+cubMap.GetLength(0) ; z+= chunk_size){
				for(int x=start_x; x<start_x+cubMap.GetLength(1) ; x+= chunk_size){
					world.CreateChunk(x,y,z,true);
				}
			}
		}

		for(int y=0; y<10 ; y++){
			for(int z=start_z; z<start_z+cubMap.GetLength(0); z++){
				for(int x=start_x; x<start_x+cubMap.GetLength(1); x++){
					if(y>0 && y<=5 && cubMap[z, x]==1){
						
						Color c = new Color(
								(float)(colorMap[z,x,0])/255f, 
								(float)(colorMap[z,x,1])/255f,
								(float)(colorMap[z,x,2])/255f
								, 1);
						world.SetBlock(x,y,z, new ColorBlock(c));

					}
					else if(y>0 && y<=5 && cubMap[z, x] == 0){
						world.SetBlock(x,y,z,new BlockAir());
					}
					else if(y==0){
						Color c = new Color(
							(float)(colorMap[z,x,0])/255f, 
							(float)(colorMap[z,x,1])/255f,
							(float)(colorMap[z,x,2])/255f
							, 1);
						world.SetBlock(x,y,z, new ColorBlock(c));
					}

					if(x==0 
						|| (x==cubMap.GetLength(1)-1)
						|| (z==cubMap.GetLength(0))
						|| (z==0)
						){
						Color c = new Color(
							(float)(colorMap[z,x,0])/255f, 
							(float)(colorMap[z,x,1])/255f,
							(float)(colorMap[z,x,2])/255f
							, 1);
						world.SetBlock(x,y,z, new ColorBlock(c));
					}
				}
			}
		}
	}

	float time=0f;

	public void GenFloatingIsand(){

		foreach(List<int[]> notgroups in edge_group){
			int[][] groups = notgroups.ToArray();
			System.Array.Sort(groups, 
				(e1, e2)=> {
					if(e1[0]>e2[0])
						return 1;
					else if(e1[0]==e2[0]){ 
						if(e1[1]>e2[1])
							return 1;
						else if(e1[1]==e2[1])
							return 0;
						else
							return -1;
					}
					else
						return -1;
				});
			

			int max_x = findmax(groups, 0), max_z = findmax(groups, 1);
			int min_x = findmin(groups, 0), min_z = findmin(groups, 1);

			int x_Width = max_x - min_x;
			int z_Length = max_z - min_z;

			int pHeight = Random.Range(10, 40); //postion of this chunk in y
			int chunkHeight = Random.Range(5, 10);
			
			int last_x = -1, last_z = -1;

			//work through points in this group(sorted)
			for(int index=0; index<groups.GetLength(0); index++){
				int x = groups[index][0], z = groups[index][1];

				for(int y=pHeight; y<pHeight+chunkHeight; y++){

					//remove the sharp edge of floating isand
					if((y==pHeight || y==pHeight+chunkHeight-1) && false ){
						
						if(x == max_x || x == min_x)
							world.SetBlock(x,y,z, new BlockAir());
						else if(z!= last_z){
							world.SetBlock(x,y,z, new BlockAir());
							try{
								world.SetBlock(groups[index-1][0],y,groups[index-1][1], new BlockAir());
								}catch{};
						}

					}
					else{
						Color c = new Color(
								(float)(colorMap[z,x,0])/255f, 
								(float)(colorMap[z,x,1])/255f,
								(float)(colorMap[z,x,2])/255f
								, 1);
						world.SetBlock(x,y,z, new ColorBlock(c));
					}
				}

				last_x = x; last_z = z;
			}
		}
	}

	int findmax(int[][] groupq, int index){
		int max=0;
		foreach(int[] p in groupq){
			max = p[index] > max ? p[index] : max;
		}
		return max;
	}

	int findmin(int[][] groupq, int index){
		int min=0;
		foreach(int[] p in groupq){
			min = p[index] < min ? p[index] : min;
		}
		return min;
	}
}
