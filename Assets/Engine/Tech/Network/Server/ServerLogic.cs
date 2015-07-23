using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerLogic : AbstractNetworkLogic {

	public StateManagerServer stateManager;

	public void StartServer( int maxConnections , int listenPort , string gameName ){
		uLink.Network.InitializeServer( maxConnections , listenPort );

		// TODO: register host on master server
	}

	protected override void Awake(){
		base.Awake ();

		Messenger.AddListener<int>( Events.Server.ForceClientDisconnect , OnForceClientDisconnect );
		Messenger.AddListener<string, int> (Events.Network.AuthorizeRequest, OnAuthorizeRequest);

		new ServerNetworkEventer( this );
	}
	
	public void KillServer(){
		uLink.Network.Disconnect();
	}

	// uLink Events	

	void uLink_OnPlayerConnected( uLink.NetworkPlayer player ){
		stateManager.connections.Add(player.id , new Connection());
	}
	
	void uLink_OnPlayerDisconnected( uLink.NetworkPlayer player ){
		Messenger.Broadcast (Events.Server.PlayerDisconnected, player.id);
		stateManager.connections.Remove (player.id);
	}

	void uLink_OnServerInitialized(){
		Messenger.Broadcast( Events.Application.Configured );
	}

	// internal events

	private void OnAuthorizeRequest(string _, int id){
		if( stateManager.CanAuthorize() ){
			Messenger.Broadcast( Events.Server.Authorized , id );
		}else{
			OnForceClientDisconnect( id );
		}
	}

	private void OnForceClientDisconnect( int id ){
		for(int i = 0 ; i < uLink.Network.connections.Length ; i++ ){
			uLink.NetworkPlayer networkPlayer = uLink.Network.connections[ i ];
			if( networkPlayer.id == id ){
				uLink.Network.CloseConnection( networkPlayer , false );
			}
		}
	}
}