using UnityEngine;
using System.Collections;

public class StateManagerClient : StateManager {
	protected override void Awake ()
	{
		base.Awake ();
		SubscribeSingleplayerEvents ();
		Messenger.Broadcast (Events.Application.Configured);
	}

	
	void uLink_OnConnectedToServer(){
		Messenger.UnmarkFromPermanent (Events.State.RegisterState);
		Messenger.Cleanup ();
		SubscribeMultiplayerEvents ();
		FixCoreEvents ();

		Messenger.Broadcast( Events.Network.SendData , Events.Network.AuthorizeRequest, "" );
	}
	
	void uLink_OnDisconnectedFromServer(){
		Messenger.UnmarkFromPermanent (Events.Network.ChangeState, Events.Network.Authorized, Events.State.RegisterState);
		Messenger.Cleanup ();
		SubscribeSingleplayerEvents ();
		FixCoreEvents ();
		ChangeState ("basketball-menu");
	}

	private void OnServerChangeState(string stateName){
		ChangeState (stateName);
	}

	private void OnAuthorized(string authorizeData){
		string[] data = Serializer.Deserialize( Separators.AuthorizeData , authorizeData );

		int connectionId = int.Parse(data [0]);
		string stateName = data [1];

		CrossStageData.Set (CrossStageEntries.myConnectionId, connectionId);
		ChangeState (stateName);
	}

	private void SubscribeSingleplayerEvents(){
		Messenger.AddListener<State>( Events.State.RegisterState , OnRegisterState );
	}

	private void SubscribeMultiplayerEvents(){
		Messenger.AddListener<string> (Events.Network.ChangeState, OnServerChangeState);
		Messenger.AddListener<string> (Events.Network.Authorized, OnAuthorized);
		Messenger.AddListener<StateMultiplayer>( Events.State.RegisterState , OnRegisterState );
	}


	private void OnRegisterStateMultiplayer(StateMultiplayer state){
		base.OnRegisterState (state);
	}
}
