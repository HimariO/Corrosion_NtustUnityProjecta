using UnityEngine;
using System.Collections;

public class ColorBlock : Block{
	public Color color;
	public ColorBlock():base(){}

	public ColorBlock(Color color)
		: base()
	{
		this.color = color;
	}
	
	public override Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		
		tile.x = 0;
		tile.y = 0;
		
		return tile;
	}
	
	protected override MeshData FaceDataUp (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		MeshData mD =  base.FaceDataUp (chunk, x, y, z, meshData);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		return mD;
	}
	
	protected override MeshData FaceDataDown (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		MeshData mD = base.FaceDataDown (chunk, x, y, z, meshData);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		return meshData;
	}
	
	protected override MeshData FaceDataNorth (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		MeshData mD = base.FaceDataNorth (chunk, x, y, z, meshData);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		return meshData;
	}
	
	protected override MeshData FaceDataSouth (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		MeshData mD = base.FaceDataSouth (chunk, x, y, z, meshData);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		return meshData;
	}
	
	protected override MeshData FaceDataEast (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		MeshData mD = base.FaceDataEast (chunk, x, y, z, meshData);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		return meshData;
	}
	
	protected override MeshData FaceDataWest (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		MeshData mD = base.FaceDataWest (chunk, x, y, z, meshData);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		mD.AddColor(color);
		return meshData;
	}
	
}
