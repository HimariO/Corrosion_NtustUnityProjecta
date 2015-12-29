using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnalyScreenShot : MonoBehaviour {

	public bool Finished = false;

	public Texture2D screenshot;
	public List<List<int[]>> r_edge_group;
	public int[,] r_edge_map;
	public int[,,] r_color_map = new int[160,90,3];
	Color[] pixels;
	Texture2D copy;

	bool show;

	void Start(){
//		StartProcess();
	}


	public void StartProcess () {

//		TextureScale.Bilinear (screenshot, 90, 160);
		copy = Instantiate(screenshot);
		pixels = copy.GetPixels();
		
		for(int i=0; i<pixels.Length; i++){
			r_color_map[(i/screenshot.width), i%screenshot.width,0] = (int)(pixels[i].r*255);
			r_color_map[(i/screenshot.width), i%screenshot.width,1] =(int)(pixels[i].g*255);
			r_color_map[i/screenshot.width, i%screenshot.width,2] =(int)(pixels[i].b*255);

			float gray = pixels[i].grayscale;
			pixels[i] = new Color(1f-gray, 1f-gray, 1f-gray, pixels[i].a);
		}
		copy.SetPixels(pixels);
		copy.Apply();
		copy = ChangeTextureContrast(copy, 0.999f);
		copy.Apply();

		AnalyTexture(copy.GetPixels());

		try{
			SaveLoad.SaveColorMap(r_color_map);
			SaveLoad.SaveEdgeMap(r_edge_map);
			SaveLoad.SaveEdgeGroup(r_edge_group);
		}catch{};


		Finished = true;

		MapFromScreenshot MFS = GetComponent<MapFromScreenshot>();
		if(MFS!=null && MFS!=null){ //try to move map infront camera(in map creating page)
			
			transform.position =  new Vector3(-113f, -190f, -354f);
			transform.eulerAngles = new Vector3(-59f/360f, -26f/360f, -90f/360f);
			transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
		}
		// -------------------------------------for debug GUI ---------------------------------------------------------
		for(int i =0;i<pixels.Length; i++){
			pixels[i] = Color.black;
			if(r_edge_map[i/copy.width, i%copy.width]==1)
				pixels[i] = Color.white;
		}
		
		foreach(List<int[]> group in r_edge_group){
			float r_color = Random.Range(0f,1f);
			float r_color_g = Random.Range(0f,1f);
			float r_color_b = Random.Range(0f,1f);

			foreach(int[] point in group){
				pixels[point[1]*copy.width + point[0]] = new Color(r_color, r_color_g, r_color_b);
			}
		}
		copy.SetPixels(pixels);
		copy.Apply();
		show = false;

		//-------------------------------------------------------------------------------------------------------------
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
	
		GUI.DrawTexture(new Rect(0, 0, 180, 320), copy);
	}
	

	void AnalyTexture(Color[] colors){
		float color_standar = 0.15f;
		int thershold = (int)(colors.Length*0.001f);

		List<List<float[]>> pixel_data = new List<List<float[]>>(); //[y][x][hsv]

		for(int y=0; y<screenshot.height ;y++){
			pixel_data.Add(new List<float[]>());
			for(int x=0; x<screenshot.width ;x++){
				float h, s, v;

				HSVFromColor(colors[y*screenshot.width+x],out h, out s, out v);
//				Debug.Log(h+" "+s+" "+v);
				pixel_data[y].Add(new float[]{h,s,v});

			}
		}


		List<int[]> edge_position = new List<int[]>();
		int[,] edge_map = new int[screenshot.height,screenshot.width]; //[y,x]

		int[,] Lookup_Offset  = new int[,]{{1, 0}, {0, -1}, {0, 1}, {1, 1}};
		Debug.Log (Lookup_Offset.GetLength(0));
		while(true){
			edge_position.Clear();

			for(int y=0; y<screenshot.height ;y++){
				for(int x=0; x<screenshot.width ;x++){

					float h = pixel_data[y][x][0], s = pixel_data[y][x][1], v = pixel_data[y][x][2];

					for(int i=0; i<Lookup_Offset.GetLength(0); i++){
						int[] vec = new int[]{Lookup_Offset[i,0], Lookup_Offset[i,1]};
						try{
							float n_h = pixel_data[y+vec[1]][x+vec[0]][0];
							float n_s = pixel_data[y+vec[1]][x+vec[0]][1];
							float n_v = pixel_data[y+vec[1]][x+vec[0]][2];

							if(Mathf.Abs(n_s-s)+Mathf.Abs(n_v-v) > color_standar || Mathf.Abs (n_h-h) >0.05){
								edge_map[y,x] = 1;
								edge_position.Add(new int[]{x,y});
								break;
							}else{
								edge_map[y,x] = 0;
							}

						}catch{
							continue;
						}
					}
				}
			}

			if(edge_position.Count<screenshot.height*screenshot.width*0.4f || color_standar>0.35f)
				break;
			color_standar += 0.01f;
		}

		r_edge_map = edge_map;

		List<List<int[]>> edge_grouped = new List<List<int[]>>();
		while(edge_position.Count>0){
			int[] coord = edge_position[0];
			List<int[]> group = new List<int[]>();
			List<int[]> cameform = new List<int[]>();

			Debug.Log("------------Search start------------");
			FindAround(coord, ref cameform,ref group);
			Debug.Log("------------Search end------------");

			if(group.Count>thershold){
				edge_grouped.Add(group);
			}

			try{
				edge_position.Remove(coord);}
			catch{
				Debug.Log("remove fail");
			}


			foreach(int[] p in group){
				try{
					foreach(int[] arr in edge_position){
						if(p[0] == arr[0] && p[1] == arr[1]){
							edge_position.Remove(arr);
							break;
						}
					}
				}
				catch{
					Debug.Log("remove fail in group");
				}
			}


			cameform.Clear();
			Debug.Log("after:"+edge_position.Count);
		}

		Debug.Log("find group:"+edge_grouped.Count);
		r_edge_group = edge_grouped;

		show = true;
	}


	void FindAround(int[] start_pos, ref List<int[]> camefrom,ref List<int[]> Allpos){
		//FindAround({x,y}, list[{x,y},{x,y}...all edge coordinate], list[{x,y},...edges alreay go through])
		int[,] offset  = new int[,]{{0, 1}, {1, 0}, {-1, 0}, {0, -1}}; //, {-1, 1}, {-1, -1}, {1, -1}, {1, 1}
//		HashSet<int[]> AllPos = new HashSet<int[]>{start_pos};
		Allpos.Add(start_pos);
		camefrom.Add(start_pos);
		int x = start_pos[0], y = start_pos[1];


		Debug.Log("Debug Start("+x+","+y);
		for(int i=0; i<offset.GetLength(0); i++){

			int[] n_p = new int[]{x+offset[i,0], y+offset[i,1]};
			bool contain = false;

			foreach(int[] cam in camefrom)
				if(cam[0]==n_p[0] && n_p[1]==cam[1]){
	
					contain = true;break;
				}

			if(!contain){
				try{
					if(r_edge_map[n_p[1], n_p[0]] == 1){
						if(offset[i,1]!=0){
							Debug.Log("find in up ro down!!");
							Debug.Log("("+x+","+y+")>("+offset[i,0]+","+offset[i,1]);

						}

						Debug.Log("Allpos bef:"+Allpos.Count);
						Allpos.Add(n_p);

						FindAround(n_p, ref camefrom, ref Allpos);
						Debug.Log("Allpos aft:"+Allpos.Count);
						
					}
				}catch{continue;}
			}
		}
//		return AllPos;
	}


	List<int[]> removeDup(List<int[]> all, HashSet<int[]> ele){
		List<int[]> result = new List<int[]>();
		foreach(int[] arr in all){
			bool repeat = false;
			foreach(int[] earr in ele){
				if(earr[0] == arr[0] && earr[1] == arr[1]){
					repeat = true;
					break;
				}
			}

			if(!repeat)
				result.Add(arr);
		}

		return result;
	}
//	HashSet<int[]> FindAround(int[] start_pos, int[,] edge_map, HashSet<int[]> camefrom){
//		//FindAround({x,y}, list[{x,y},{x,y}...all edge coordinate], list[{x,y},...edges alreay go through])
//		int[,] offset  = new int[,]{{1, 0}, {0, -1}, {-1, 0}, {0, 1}};
//		HashSet<int[]> AllPos = new HashSet<int[]>{start_pos};
//		int x = start_pos[0], y = start_pos[1];
//
//		if(camefrom.Count<=1){ // if this is startpoint
//			try{
//				if(edge_map[y+offset[0,1], x+offset[0,0]] == 0
//				   && edge_map[y+offset[1,1], x+offset[1,0]] == 0
//				   && edge_map[y+offset[2,1], x+offset[2,0]] == 0
//				   && edge_map[y+offset[3,1], x+offset[3,0]] == 0)
//					return new HashSet<int[]>{};
//			}catch{}
//		}
//
//		for(int i=0; i<offset.GetLength(0); i++){
//			int[] n_p = new int[]{x+offset[i,0], y+offset[i,1]};
//
//			if(!camefrom.Contains(n_p)){
//				try{
//					if(edge_map[n_p[1], n_p[0]] == 1)
//						camefrom.UnionWith(AllPos);
//						AllPos.UnionWith(FindAround(n_p, edge_map, camefrom));
//				}catch{continue;}
//			}
//		}
//		return AllPos;
//	}

	public static void HSVFromColor(Color color, out float h, out float s, out float bb)
	{
		
		
		float r = color.r;
		float g = color.g;
		float b = color.b;
		
		float max = Mathf.Max(r, Mathf.Max(g, b));
		
		float min = Mathf.Min(r, Mathf.Min(g, b));
		float dif = max - min;
		
		if (max > min)
		{
			if (g == max)
			{
				h = (b - r) / dif * 60f + 120f;
			}
			else if (b == max)
			{
				h = (r - g) / dif * 60f + 240f;
			}
			else if (b > g)
			{
				h = (g - b) / dif * 60f + 360f;
			}
			else
			{
				h = (g - b) / dif * 60f;
			}
			if (h < 0)
			{
				h = h + 360f;
			}
		}
		else
		{
			h = 0;
		}
		
		h *= 1f / 360f;
		s = (dif / max) * 1f;
		bb = max;
	}


	public static Texture2D ChangeTextureContrast(Texture2D originalTexture, float power) {
		if(power<0f) power=1f;
		Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
		Color[] originalPixels = originalTexture.GetPixels(0);
		Color[] newPixels = newTexture.GetPixels(0);
		float[] avgColor = new float[3];
		for (int i = 0; i < originalPixels.Length; i++) {
			Color c = originalPixels[i];
			avgColor[0]+=c.r;
			avgColor[1]+=c.g;
			avgColor[2]+=c.b;
		}
		avgColor[0] = avgColor[0] / originalPixels.Length;
		avgColor[1] = avgColor[1] / originalPixels.Length;
		avgColor[2] = avgColor[2] / originalPixels.Length;
		
		for (int i = 0; i < originalPixels.Length; i++) {
			Color c = originalPixels[i];
			float deltaR = c.r - avgColor[0];
			float deltaG = c.g - avgColor[1];
			float deltaB = c.b - avgColor[2];
			deltaR = Mathf.Pow(Mathf.Abs(deltaR), power) * Mathf.Sign(deltaR);
			deltaG = Mathf.Pow(Mathf.Abs(deltaG), power) * Mathf.Sign(deltaG);
			deltaB = Mathf.Pow(Mathf.Abs(deltaB), power) * Mathf.Sign(deltaB);
			newPixels[i] = new Color(avgColor[0] + deltaR, 
			                         avgColor[1] + deltaG, 
			                         avgColor[2] + deltaB, 
			                         c.a);
		}
		newTexture.SetPixels(newPixels, 0);
		newTexture.Apply();
		return newTexture;
	}
}
