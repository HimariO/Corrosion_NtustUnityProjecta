#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class TestSocketIO : MonoBehaviour
{
	GameObject Player;
	public GameObject other_prefab;
	private SocketIOComponent socket;
	private Modify modify;
	private World world;

	const string ADD_USER = "adduser";
	const string UPDATE_POS = "update_position";
	public Dictionary<string,GameObject> other_players = new Dictionary<string,GameObject>();
	float delay=0;

	public void Start() 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		Player = GameObject.FindGameObjectWithTag("Player");
		modify = Player.GetComponent<Modify>();
		world = GameObject.Find("WorldStartHere").GetComponent<World>();

		socket.On("open", TestOpen);
		socket.On("boop", TestBoop);
		socket.On("error", TestError);
		socket.On("close", TestClose);
		socket.On("user_added", AddUserToWorld);
		socket.On ("user_update_position", UpdateOtherPos);
		socket.On ("user list", AddUserToWorld);
		socket.On("map_modify", ModifyMap);
		socket.On ("set_specblock", SetSpecBlock);


		StartCoroutine("BeepBoop");
//		StartCoroutine("SendPosition");
	}

	public void Update(){
		delay +=Time.deltaTime;
		if(delay >0.025f){
			SendPosition();
			delay=0;
		}
//		print("socktest updata");
	}

	private IEnumerator BeepBoop()
	{
		
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		socket.Emit("beep");
		Dictionary<string,string> data = new Dictionary<string ,string>();
		data["id"] = ""+socket.sid;
		data["p_x"] = ""+Player.transform.position.x;
		data["p_y"] = ""+Player.transform.position.y;
		data["p_z"] = ""+Player.transform.position.z;
		data["rota_x"] = ""+Player.transform.rotation.x;
		data["rota_y"] = ""+Player.transform.rotation.y;
		data["rota_z"] = ""+Player.transform.rotation.z;
		
		socket.Emit(ADD_USER, new JSONObject(data));
		
		// wait 2 seconds and continue
		yield return new WaitForSeconds(2);
		
		socket.Emit("beep");
		SendPosition();
		// wait ONE FRAME and continue
		yield return null;
	}

	private void SendPosition()
	{
		Dictionary<string,string> data = new Dictionary<string ,string>();
		data["id"] = ""+socket.sid;
		data["p_x"] = ""+Player.transform.position.x;
		data["p_y"] = ""+Player.transform.position.y;
		data["p_z"] = ""+Player.transform.position.z;
		data["rota_x"] = ""+Player.transform.rotation.x;
		data["rota_y"] = ""+Player.transform.rotation.y;
		data["rota_z"] = ""+Player.transform.rotation.z;
		
		socket.Emit(UPDATE_POS, new JSONObject(data));
		// wait 1 seconds and continue
//		yield return new WaitForSeconds(0.1f);
	}


	public void SendModifyMap(Vector3 position, string type){
		Dictionary<string,string> data = new Dictionary<string ,string>();
		data["id"] = ""+socket.sid;
		data["p_x"] = ""+position.x;
		data["p_y"] = ""+position.y;
		data["p_z"] = ""+position.z;
		data["type"] = type;
		socket.Emit("map_modify", new JSONObject(data));

	}

	public void SendSetSpecBlock(WorldPos position, BlockSpecial.Specal_type type){
		Dictionary<string,string> data = new Dictionary<string ,string>();
		data["id"] = ""+socket.sid;
		data["p_x"] = ""+position.x;
		data["p_y"] = ""+position.y;
		data["p_z"] = ""+position.z;
		data["type"] = type.ToString();
		socket.Emit("set_specblock", new JSONObject(data));
	}


	public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		Debug.Log(
			"#####################################################" +
			"THIS: " + e.data.GetField("this").str +
			"#####################################################"
		);
	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO]  received: " + e.name + " " + e.data);
	}

	public void AddUserToWorld(SocketIOEvent e){
		Debug.Log("[SocketIO] ADD USER received: " + e.name + " " + e.data);
		Vector3 test = new Vector3(0f,0f,0f) ;

		Dictionary<string,string> data = e.data.ToDictionary();
		Vector3 n_position = new Vector3(float.Parse(data["p_x"]), float.Parse(data["p_y"]), float.Parse(data["p_z"]));
		Quaternion n_rotation = Quaternion.Euler(float.Parse(data["rota_x"]), float.Parse(data["rota_y"]), float.Parse(data["rota_z"]));
		other_players.Add(data["id"], Instantiate(other_prefab, n_position, n_rotation) as GameObject);


	}

	public void AddCurrentUsers(SocketIOEvent e){
//		e.data.ToDictionary
	}

	Dictionary<string, System.DateTime> lastTime;
	public void UpdateOtherPos(SocketIOEvent e){
//		Debug.Log("[SocketIO]  received: " + e.name + " " + e.data);
		System.DateTime time=System.DateTime.Now;

		Dictionary<string,string> data = e.data.ToDictionary();
		Vector3 n_position = new Vector3(float.Parse(data["p_x"]), float.Parse(data["p_y"]), float.Parse(data["p_z"]));
		Quaternion n_rotation = Quaternion.Euler(float.Parse(data["rota_x"]), float.Parse(data["rota_y"]), float.Parse(data["rota_z"]));
		GameObject other;

		if(other_players.TryGetValue(data["id"], out other)){
			Vector3 v= other.GetComponent<Rigidbody>().velocity;
			other.transform.position = Vector3.SmoothDamp(
				other.transform.position, 
				n_position,
				ref v,
				0.025f);
			other.transform.rotation= Quaternion.Slerp(other.transform.rotation, n_rotation, Time.deltaTime*2f);
		}
		else{
			AddUserToWorld(e);
		}

		lastTime[data["id"]] = time;
	}


	public void ModifyMap(SocketIOEvent e){
		Debug.Log("[SocketIO]  received: " + e.name + " " + e.data);
		Dictionary<string,string> data = e.data.ToDictionary();
		Vector3 n_position = new Vector3(float.Parse(data["p_x"]), float.Parse(data["p_y"]), float.Parse(data["p_z"]));

		switch(data["type"]){
		case "BlockMap":
			modify.ReplaceBlock(n_position);
			break;

		default:
			if(data["type"].StartsWith("BlockSpecial_")){
				int type = int.Parse(data["type"].Substring(data["type"].Length-1, 1)); //future bug :D
				modify.ReplaceBlock(n_position, type);
			}
			break;
		}
	}


	public void SetSpecBlock(SocketIOEvent e){
		Dictionary<string,string> data = e.data.ToDictionary();
		WorldPos WS = new WorldPos((int)float.Parse(data["p_x"]), (int)float.Parse(data["p_y"]), (int)float.Parse(data["p_z"]));
		BlockSpecial.Specal_type type;
		switch(data["type"]){
		case "Bound":
			type = BlockSpecial.Specal_type.Bound;
			world.SpecialBlockEff(WS, type, BlockSpecial.Owner.other);
			break;
		case "Suck":
			type = BlockSpecial.Specal_type.Suck;
			world.SpecialBlockEff(WS, type, BlockSpecial.Owner.other);
			break;
		case "Teleport":
			type = BlockSpecial.Specal_type.Teleport;
			world.SpecialBlockEff(WS, type, BlockSpecial.Owner.other);
			break;
		}
	}
}
