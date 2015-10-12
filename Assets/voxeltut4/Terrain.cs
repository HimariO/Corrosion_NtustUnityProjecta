using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Terrain
{
    public static WorldPos GetBlockPos(Vector3 pos)
    {
        WorldPos blockPos = new WorldPos(
            Mathf.RoundToInt(pos.x),
            Mathf.RoundToInt(pos.y),
            Mathf.RoundToInt(pos.z)
            );

        return blockPos;
    }

    public static WorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
    {
        Vector3 pos = new Vector3(
            MoveWithinBlock(hit.point.x, hit.normal.x, adjacent),
            MoveWithinBlock(hit.point.y, hit.normal.y, adjacent),
            MoveWithinBlock(hit.point.z, hit.normal.z, adjacent)
            );



        return GetBlockPos(pos);
    }

    static float MoveWithinBlock(float pos, float norm, bool adjacent = false)
    {
        if (pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
        {
            if (adjacent)
            {
                pos += (norm / 2);
            }
            else
            {
                pos -= (norm / 2);
            }
        }

        return (float)pos;
    }

    public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return false;

        WorldPos pos = GetBlockPos(hit, adjacent);

        chunk.world.SetBlock(pos.x, pos.y, pos.z, block);

        return true;
    }


	public static List<Vector3> Explosion(RaycastHit hit, Block block, bool adjacent = false)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk>();
		if (chunk == null)
			return null;

		int range = 2;
		WorldPos pos = GetBlockPos(hit, adjacent);
		List<Vector3> prefab_pos = new List<Vector3>();

		for (int x=-range; x<range; x++)
			for (int y=-range; y<range; y++)
				for (int z=-range; z<range; z++) {
					
					if(!(chunk.world.GetBlock(pos.x+x, pos.y+y, pos.z+z) is BlockAir)){
						prefab_pos.Add(new Vector3(pos.x+x, pos.y+y, pos.z+z));
						chunk.world.SetBlock(pos.x+x, pos.y+y, pos.z+z, block);
					}
				}

		
		return prefab_pos;
	}


    public static Block GetBlock(RaycastHit hit, bool adjacent = false)
    {
        Chunk chunk = hit.collider.GetComponent<Chunk>();
        if (chunk == null)
            return null;

        WorldPos pos = GetBlockPos(hit, adjacent);

        Block block = chunk.world.GetBlock(pos.x, pos.y, pos.z);

        return block;
    }


}