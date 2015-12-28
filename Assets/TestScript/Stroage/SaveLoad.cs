

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad {
	
//	public static List<Game> savedGames = new List<Game>();
	
	//it's static so we can call it from anywhere
	public static void SaveEdgeGroup(List<List<int[]>> data) {
		List<ListWrapper> container = new List<ListWrapper>();

		foreach(List<int[]> ele in data){
			container.Add(new ListWrapper(ele));
		}

//		SaveLoad.savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/EdgeGroup.gd"); //you can call it anything you want
		bf.Serialize(file, data);
//		Debug.Log(bf.);
		file.Close();
	}   
	
	public static List<List<int[]>> LoadEdgeGroup() {
		if(File.Exists(Application.persistentDataPath + "/EdgeGroup.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/EdgeGroup.gd", FileMode.Open);
			List<ListWrapper> readHash = (List<ListWrapper>)bf.Deserialize(file);
			List<List<int[]>> container = new List<List<int[]>>();

			foreach(ListWrapper ele in readHash)
				container.Add(ele.data);

			file.Close();
			return container;
		}
		return new List<List<int[]>>();
	}

	public static void SaveColorMap(int[,,] data) {
		
		//		SaveLoad.savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/ColorMap.gd"); //you can call it anything you want
		bf.Serialize(file, data);
		//		Debug.Log(bf.);
		file.Close();
	}   
	
	public static int[,,] LoadColorMap() {
		if(File.Exists(Application.persistentDataPath + "/ColorMap.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/ColorMap.gd", FileMode.Open);
			int[,,] colormap = (int[,,])bf.Deserialize(file);
			
			file.Close();
			return colormap;
		}

		return new int[,,]{};
	}


	public static void SaveEdgeMap(int[,] data) {
		
		//		SaveLoad.savedGames.Add(Game.current);
		BinaryFormatter bf = new BinaryFormatter();
		//Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
		FileStream file = File.Create (Application.persistentDataPath + "/EdgeGroup.gd"); //you can call it anything you want
		bf.Serialize(file, data);
		//		Debug.Log(bf.);
		file.Close();
	}   
	
	public static int[,] LoadEdgeMap() {
		if(File.Exists(Application.persistentDataPath + "/EdgeGroup.gd")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/EdgeGroup.gd", FileMode.Open);
			int[,] readHash = (int[,])bf.Deserialize(file);
			
			file.Close();
			return readHash;
		}
		return new int[,]{};
	}

}