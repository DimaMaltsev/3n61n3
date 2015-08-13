using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ServersManagerInterface : MonoBehaviour {

	private ServerLogic logic;

	private string serverKey;

	public string managerIp = "http://127.0.0.1:8080/";
	public string registerServer = "registerServer";
	public string updateServer = "updateServer";


	void Awake(){
		logic = GetComponent<ServerLogic> ();

		Messenger.AddListener<string> (Events.Server.RegisteredServer, OnRegisteredServer);
		Messenger.AddListener<int> (Events.Server.UpdateServerInfo, OnUpdateServerInfo);
	}

	private void OnRegisteredServer(string text){
		JSONObject json = new JSONObject (text);
		serverKey = json.GetField ("key").str;
		
		print (serverKey);
		CrossStageData.Set (CrossStageEntries.serverKey, serverKey);
	}

	public void RegisterServer(string ip, int port, string gameName){
		JSONObject json = new JSONObject ();

		json.AddField ("ip", ip);
		json.AddField ("port", port);
		json.AddField ("name", gameName);

		logic.SendAjax (managerIp + registerServer, json.ToString (), Events.Server.RegisteredServer);
	}

	public void OnUpdateServerInfo(int playersCount){
		JSONObject json = new JSONObject ();

		json.AddField ("playersCount", playersCount);
		json.AddField ("key", serverKey);

		logic.SendAjax (managerIp + updateServer, json.ToString ());
	}
}
