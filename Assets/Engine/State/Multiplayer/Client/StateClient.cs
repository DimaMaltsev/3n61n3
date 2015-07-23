using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateClient : StateMultiplayer {
	public override void Awake(){
		base.Awake ();

		Messenger.AddListener<string> (Events.Network.StateInitData, OnStateInitData);
		Messenger.AddListener<string> (Events.Network.DataStorageUpdate, OnDataStorageUpdate);

		Messenger.Broadcast (Events.Network.SendData , Events.Network.StateInitDataRequest , "");
	}

	private void OnStateInitData(string data){
		ApplyInitData (data);
		Resolved ();
	}

	private void OnDataStorageUpdate(string data){
		ApplyUpdates (data);
	}

	public override void Resolved(){
		Messenger.Broadcast (Events.Network.SendData, Events.State.ClientResolvedState, "");
	}

	public override void BroadcastUpdates (string updates)
	{
		Messenger.Broadcast( Events.Network.SendData , Events.Network.DataStorageUpdate , updates );
	}
}
