using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]

public class World : MonoBehaviour {

    public Dictionary<WorldPos, Chunk> chunks = new Dictionary<WorldPos, Chunk>();
    public GameObject chunkPrefab;

    void Start()
    {
		
    }

	// Update is called once per frame
	void Update () {
	
	}

	public void CreateChunk(int x, int y, int z)
    {
        WorldPos worldPos = new WorldPos(x, y, z);
//		print ("create chunk"+worldPos.ToString());

		int chunk_size = Chunk.chunkSize;

        //Instantiate the chunk at the coordinates using the chunk prefab
        GameObject newChunkObject = Instantiate(
                        chunkPrefab, new Vector3(x, y, z),
                        Quaternion.Euler(Vector3.zero)
                    ) as GameObject;

		//Getting Chunk.cs in Chunk Prefab.
        Chunk newChunk = newChunkObject.GetComponent<Chunk>();

        newChunk.pos = worldPos;
        newChunk.world = this;

        //Add it to the chunks dictionary with the position as the key
        chunks.Add(worldPos, newChunk);

        for (int xi = 0; xi < chunk_size; xi++)
        {
			for (int yi = 0; yi < chunk_size; yi++)
            {
				for (int zi = 0; zi < chunk_size; zi++)
                {
					SetBlock(x + xi, y + yi, z + zi, new BlockAir());
                }
            }
        }

//		SetBlockS(GeneratePillars(new WorldPos(x,y,z), Chunk.chunkSize));
    }


    public void DestroyChunk(int x, int y, int z)
    {
        Chunk chunk = null;
        if (chunks.TryGetValue(new WorldPos(x, y, z), out chunk))
        {
            Object.Destroy(chunk.gameObject);
            chunks.Remove(new WorldPos(x, y, z));
        }
    }


    public Chunk GetChunk(int x, int y, int z)
    {
        WorldPos pos = new WorldPos();
        float multiple = Chunk.chunkSize;
        pos.x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize;
        pos.y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize;
        pos.z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize;

        Chunk containerChunk = null;

        chunks.TryGetValue(pos, out containerChunk);
//		if(containerChunk==null)
//			print (pos.ToString());
        return containerChunk;
    }

    public Block GetBlock(int x, int y, int z)
    {
        Chunk containerChunk = GetChunk(x, y, z);

        if (containerChunk != null)
        {
            Block block = containerChunk.GetBlock(
                x - containerChunk.pos.x,
                y - containerChunk.pos.y,
                z - containerChunk.pos.z);

            return block;
        }
        else
        {
            return new BlockAir();
        }

    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        Chunk chunk = GetChunk(x, y, z);

        if (chunk != null)//<-- some position will make chunk not findable?
        {
            chunk.SetBlock(x - chunk.pos.x, y - chunk.pos.y, z - chunk.pos.z, block);
            chunk.update = true;

            UpdateIfEqual(x - chunk.pos.x, 0, new WorldPos(x - 1, y, z));
            UpdateIfEqual(x - chunk.pos.x, Chunk.chunkSize - 1, new WorldPos(x + 1, y, z));
            UpdateIfEqual(y - chunk.pos.y, 0, new WorldPos(x, y - 1, z));
            UpdateIfEqual(y - chunk.pos.y, Chunk.chunkSize - 1, new WorldPos(x, y + 1, z));
            UpdateIfEqual(z - chunk.pos.z, 0, new WorldPos(x, y, z - 1));
            UpdateIfEqual(z - chunk.pos.z, Chunk.chunkSize - 1, new WorldPos(x, y, z + 1));
        
        }
    }

	//cuurent this method are ""colorblock only""!
	public void SetBlockS(List<WorldPos> coordList){

		for(int i=0;i<coordList.Count;i++){
			System.Random r = new System.Random();
			Color c = new Color((float)r.NextDouble(),(float)r.NextDouble(),(float)r.NextDouble()); 
			
			SetBlock(
				coordList[i].x,
				coordList[i].y,
				coordList[i].z,
				new ColorBlock(c));
		}
	}

	public void SetSpecalBlockS(List<WorldPos> coordList, BlockSpecial.Specal_type btype, BlockSpecial.Owner owner){
		
		for(int i=0;i<coordList.Count;i++){
			System.Random r = new System.Random();
			Color c = new Color((float)r.NextDouble(),(float)r.NextDouble(),(float)r.NextDouble()); 
//			BlockSpecial a=new BlockSpecial(btype);
			SetBlock(
				coordList[i].x,
				coordList[i].y,
				coordList[i].z,
				new BlockSpecial(btype, owner));
		}
	}


    void UpdateIfEqual(int value1, int value2, WorldPos pos)
    {
        if (value1 == value2)
        {
            Chunk chunk = GetChunk(pos.x, pos.y, pos.z);
            if (chunk != null)
                chunk.update = true;
        }
    }


//	List<WorldPos> GeneratePillars(WorldPos startpoint,int size){
//		int pillar_num = Random.Range (15, 30);
//
//		List<WorldPos> base_position = new List<WorldPos>();
//		for (int i=0; i<pillar_num; i++) {
//			WorldPos w = new WorldPos(
//				Random.Range(0,size),
//				(int)Random.Range(0,size*0.7f),
//				Random.Range(0,size)
//				);
//			w.extra = (int)Random.Range(3,6);
//			base_position.Add(w);
//		}
//
//		List<WorldPos> points = new List<WorldPos> ();
//
//		for (int i=0; i<base_position.Count; i++) {
//			WorldPos base_point = base_position[i];
//
//			for(int h=0;h<base_point.y;h++){
//				for(int x=0;x<base_point.extra;x++)
//					for(int z=0;z<base_point.extra;z++){
//					points.Add(
//						new WorldPos(
//						startpoint.x+base_point.x+x,
//						startpoint.y+h,
//						startpoint.z+base_point.z+z
//						)
//			       );
//				}
//			}
//		}
//
//		return points;
//	}

	public void SpecialBlockEff(WorldPos position, BlockSpecial.Specal_type blocktype, BlockSpecial.Owner owner){
		List<WorldPos> points = new List<WorldPos> ();
		int range = 8;
		for(int x=-range/2; x<range/2; x++)
			for(int y=-range/2; y<range/2; y++)
				for(int z=-range/2; z<range/2; z++){
					
				if(!(GetBlock(position.x+x, position.y+y, position.z+z) is BlockAir))
					points.Add(
						new WorldPos(
						position.x+x,
						position.y+y,
						position.z+z
						)
					);
			}

		SetSpecalBlockS(points, blocktype, owner);

	}
}
