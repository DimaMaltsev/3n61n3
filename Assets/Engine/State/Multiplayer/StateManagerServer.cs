using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManagerServer : StateManager {
	protected StateMultiplayer multiplayerCurrentState;
	
	public Dictionary<int, Connection> connections = new Dictionary<int, Connection>();

	public bool CanAuthorize(){
		return multiplayerCurrentState.CanAuthorize();
	}

	protected override void Awake ()
	{
		base.Awake ();
		CrossStageData.Set (CrossStageEntries.myConnectionId, -1);

		Messenger.AddListener<int> (Events.Network.Authorized, OnAuthorized);
		Messenger.AddListener<int> (Events.Server.SentInitDataToClient, OnSentInitDataToClient);
		Messenger.AddListener<StateMultiplayer>( Events.State.RegisterState , OnRegisterState );
	}

	public override void PreLoadState (string stateName)
	{
		if( multiplayerCurrentState != null )
			multiplayerCurrentState.WriteStateDataToCrossStorage ();

		Messenger.Broadcast( Events.Server.SendDataAll, Events.Network.ChangeState, stateName );
		foreach(KeyValuePair<int, Connection> pair in connections)
			pair.Value.resolved = false;
	}

	public void SendAuthorizeData(int connectionId){
		string data = Serializer.Serialize( Separators.AuthorizeData, connectionId , currentStateName );
		Messenger.Broadcast (Events.Network.SendData, connectionId, Events.Network.Authorized, data);
	}

	private void OnSentInitDataToClient(int id){
		connections [id].resolved = true;
	}

	private void OnAuthorized(int id){
		SendAuthorizeData (id);
	}

	private void OnRegisterState(StateMultiplayer state){
		multiplayerCurrentState = state;
		base.OnRegisterState (state);
	}
}
