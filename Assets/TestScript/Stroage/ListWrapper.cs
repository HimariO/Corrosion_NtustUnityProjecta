using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ListWrapper{

	public List<int[]> data;

	public ListWrapper(List<int[]> data){this.data = data;}
}
