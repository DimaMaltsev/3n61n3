using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class ServersManagerInterfaceClient : MonoBehaviour {

	private ClientLogic logic;

	private string serverKey;

	public string managerIp = "http://127.0.0.1:8080/";
	public string serversList = "serversList";

	void Awake(){
		logic = GetComponent<ClientLogic> ();

		Messenger.AddListener (Events.Client.FetchServersList, OnFetchServersList);
	}

	private void OnFetchServersList(){
		logic.SendAjax (managerIp + serversList, "{}", Events.Client.ServersListUpdated);
	}
}
