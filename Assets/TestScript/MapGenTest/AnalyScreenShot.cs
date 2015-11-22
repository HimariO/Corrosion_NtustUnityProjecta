using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnalyScreenShot : MonoBehaviour {

	public Texture2D screenshot;
	public List<HashSet<int[]>> r_edge_group;
	public int[,] r_edge_map;
	public int[,,] r_color_map = new int[160,90,3];
	Color[] pixels;
//	Texture2D after_process;
	// Use this for initialization
	void Start () {
//		 dscreenshot.Resize(115,204);
//		screenshot.Apply();
	

//		TextureScale.Bilinear (screenshot, 90, 160);
		pixels = screenshot.GetPixels();
		
		for(int i=0; i<pixels.Length; i++){
			r_color_map[(i/screenshot.width), i%screenshot.width,0] = (int)(pixels[i].r*255);
			r_color_map[(i/screenshot.width), i%screenshot.width,1] =(int)(pixels[i].g*255);
			r_color_map[i/screenshot.width, i%screenshot.width,2] =(int)(pixels[i].b*255);

			int p = ((256 * 256 + (int)(pixels[i].r*256)) * 256 + (int)(pixels[i].b*256f)) * 256 + (int)(pixels[i].g*256);
			int b = p % 256;
			p = Mathf.FloorToInt(p / 256);
			int g = p % 256;
			p = Mathf.FloorToInt(p / 256);
			int r = p % 256;
			float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
			Color c = new Color(l, l, l, 1);

			float gray = pixels[i].grayscale;
			pixels[i] = new Color(gray, gray, gray, pixels[i].a);
//			pixels[i] = c;
		}
		AnalyTexture(pixels);

		GetComponent<MapFromScreenshot>().GenMapBase();
		GetComponent<MapFromScreenshot>().GenFloatingIsand();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		Texture2D a= Instantiate(screenshot);
		a.SetPixels(pixels);
		a.Apply();
//		GUI.DrawTexture(new Rect(0, 0, 180, 320), a);
	}
	

	void AnalyTexture(Color[] colors){
		float color_standar = 0.08f;
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

		int[,] Lookup_Offset  = new int[,]{{1, 0}, {0, -1}, {1, -1}, {1, 1}, {0, 1}};

		while(true){
			edge_position.Clear();

			for(int y=0; y<screenshot.height ;y++){
				for(int x=0; x<screenshot.width ;x++){
					float h = pixel_data[y][x][0], s = pixel_data[y][x][1], v = pixel_data[y][x][2];

					for(int i=0; i<Lookup_Offset.GetLength(0); i++){
						int[] vec = new int[]{Lookup_Offset[0,0], Lookup_Offset[0,1]};
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

		List<HashSet<int[]>> edge_grouped = new List<HashSet<int[]>>();
		while(edge_position.Count>0){
			int[] coord = edge_position[0];

			HashSet<int[]> group = FindAround(coord, new HashSet<int[]>{coord});
			Debug.Log("group size:"+group.Count+"::"+thershold);
			if(group.Count>thershold){
				edge_grouped.Add(group);Debug.Log ("group count:"+edge_grouped.Count);
			}
			try{edge_position.Remove(coord);}catch{}

			foreach(int[] p in group)
				try{edge_position.Remove(p);}catch{continue;}
		}

		r_edge_group = edge_grouped;

	}


	HashSet<int[]> FindAround(int[] start_pos, HashSet<int[]> camefrom){
		//FindAround({x,y}, list[{x,y},{x,y}...all edge coordinate], list[{x,y},...edges alreay go through])
		int[,] offset  = new int[,]{{1, 0}, {0, -1}, {-1, 0}, {0, 1}};
		HashSet<int[]> AllPos = new HashSet<int[]>{start_pos};
		int x = start_pos[0], y = start_pos[1];
		
		if(camefrom.Count<=1){ // if this is startpoint
			try{
				if(r_edge_map[y+offset[0,1], x+offset[0,0]] == 0
				   && r_edge_map[y+offset[1,1], x+offset[1,0]] == 0
				   && r_edge_map[y+offset[2,1], x+offset[2,0]] == 0
				   && r_edge_map[y+offset[3,1], x+offset[3,0]] == 0)
				return new HashSet<int[]>{};
			}catch{}
		}
		
		for(int i=0; i<offset.GetLength(0); i++){
			int[] n_p = new int[]{x+offset[i,0], y+offset[i,1]};

			bool contain = false;
			foreach(int[] cam in camefrom)
				if(cam[0]==n_p[0] && cam[1]==cam[1]){
					contain = true;break;
				}
			if(!contain){
				try{
					if(r_edge_map[n_p[1], n_p[0]] == 1){
						camefrom.UnionWith(AllPos);
						AllPos.UnionWith(FindAround(n_p, camefrom));
					}
				}catch{continue;}
			}
		}
		return AllPos;
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
}
