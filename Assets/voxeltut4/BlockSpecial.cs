using UnityEngine;
using System.Collections;

public class BlockSpecial : ColorBlock {

	public Specal_type type;
	public Owner owner;
	public enum Specal_type {Bound, Teleport, Suck, Break};
	public enum Owner {me, other};
	static public string Bound="Bound", Teleport="Teleport", Suck="Suck", Break="Break";

	public BlockSpecial(Specal_type type,Owner o):base(){
		this.type = type;
		this.owner = o;

		switch(type){
		case Specal_type.Bound:
			this.color = new Color(127f/255f, 255f/255f, 0f/255f);
			break;
		case Specal_type.Break:
			this.color = new Color(255f/255f, 48f/255f, 48f/255f);
			break;
		case Specal_type.Suck:
			this.color = new Color(0f, 255f/255f, 255f/255f);
			break;
		}
	}

	public override Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		tile.x = 1;
		tile.y = 0;
		
		return tile;
	}
	
}
