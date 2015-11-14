using UnityEngine;
using System.Collections;

public class VertexColorCheck : MonoBehaviour {
	MeshFilter mf;
	Color[] colors;
	// Use this for initialization
	void Start () {
		mf = GetComponent<MeshFilter>();
		print(mf.mesh.colors.Length);
		if(mf.mesh.colors.Length!=0)
			colors = mf.mesh.colors;
		else
			colors = new Color[mf.mesh.vertices.Length];

		for(int i=0;i<colors.Length;i++){
			if(i>colors.Length-5){
				colors[i].r = 0;
				colors[i].g = 0;
				colors[i].b = 1;
				continue;
			}
			colors[i].r = 0;
			colors[i].g = 1;
			colors[i].b = 0;
		}


		mf.mesh.colors=colors;
		mf.mesh.RecalculateNormals();
		print(mf.mesh.vertices.Length+"vx len");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
