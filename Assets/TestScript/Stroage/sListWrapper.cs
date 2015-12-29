using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class sListWrapper{
	[SerializeField]
	public List<int[]> data;

	public sListWrapper(List<int[]> data){this.data = data;}
}
