using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateServer : StateMultiplayer {
	private bool resolved = false;

	private List<int> resolveWaiters = new List<int> ();

	public override void Awake (){
		base.Awake ();

		Messenger.AddListener<string, int> (Events.State.ClientResolvedState, OnClientResolvedState);
		Messenger.AddListener<int> (Events.Server.PlayerDisconnected, OnPlayerDisconnected); // TODO: i think it should be more deep logic here

		Messenger.AddListener<string, int> (Events.Network.StateInitDataRequest, OnStateInitDataRequest);
		Messenger.AddListener<string, int> (Events.Network.DataStorageUpdate, OnDataStorageUpdate);

		ProcessInitData ();

		Resolved ();
	}

	public virtual void ClientResolvedState(int id){}
	public virtual void ClientDisconnected(int id){} // TODO: obviously, we shoud get rid of this _

	private void OnClientResolvedState(string _, int id){
		Messenger.Broadcast( Events.Network.ClientResolvedState , id );
		ClientResolvedState (id);
	}

	private void OnPlayerDisconnected(int id){
		ClientDisconnected (id);
	}

	private void OnStateInitDataRequest(string _, int id){
		if(!resolved){
			resolveWaiters.Add(id);
			return;
		}

		SendInitDataToClient (id);
	}

	private void SendInitDataToClient(int id){
		Messenger.Broadcast (Events.Network.SendData, id, Events.Network.StateInitData, GetData());
		Messenger.Broadcast (Events.Server.SentInitDataToClient, id);
	}
	
	private void OnDataStorageUpdate(string updates, int clientId){
		ApplyUpdates (updates, clientId );
		Messenger.Broadcast( Events.Server.SendDataExcept , clientId,  Events.Network.DataStorageUpdate , updates );
	}

	public override void BroadcastUpdates (string updates)
	{
		Messenger.Broadcast( Events.Server.SendDataAll , Events.Network.DataStorageUpdate , updates );
	}

	public override void Resolved(){
		resolved = true;
		Debug.Log ("Server resolved state");
		for(int i = 0 ; i < resolveWaiters.Count; i++){
			int id = resolveWaiters[i];
			SendInitDataToClient(id);
		}

		resolveWaiters.Clear ();
	}

	private void ProcessInitData(){
		string initData = ReadStateDataFromCrossStorage ();

		if (initData != "") {
			RemoveAllNetworkObjects ();
			ApplyInitData (initData);
		} else {
			RegisterAllNetworkObjects();
		}
	}

	private void RegisterAllNetworkObjects(){
		GameObject[] networkObjects = GameObject.FindGameObjectsWithTag ("NetworkObject");
		for (int i = 0; i < networkObjects.Length; i++)
			networkObjects [i].GetComponent<UnityObjectNetworker> ().RegisterSelf ();
	}

	private void RemoveAllNetworkObjects(){
		GameObject[] networkObjects = GameObject.FindGameObjectsWithTag ("NetworkObject");
		for (int i = 0; i < networkObjects.Length; i++)
			networkObjects [i].GetComponent<UnityObjectNetworker> ().RemoveSelf ();
	}
}
