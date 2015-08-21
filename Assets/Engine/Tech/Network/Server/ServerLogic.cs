using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerLogic : AbstractNetworkLogic {

	public StateManagerServer stateManager;

	private ServersManagerInterface serversManager;

	private int listenPort;
	private string gameName;

	public void StartServer( int maxConnections , int listenPort , string gameName ){
		this.listenPort = listenPort;
		this.gameName = gameName;

		uLink.Network.InitializeServer( maxConnections , listenPort );
	}

	protected override void Awake(){
		base.Awake ();

		Messenger.AddListener<int>( Events.Server.ForceClientDisconnect , OnForceClientDisconnect );
		Messenger.AddListener<string, int> (Events.Network.AuthorizeRequest, OnAuthorizeRequest);

		serversManager = GetComponent<ServersManagerInterface> ();

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
		//if (!uLink.Network.HavePublicAddress ()) {
		//	Debug.LogWarning ("This server will not be visible in network as soon as it does not have public ip");
		//	Debug.LogWarning ("If you still want to connect to it, try using particular computer network ip");
		//} else {
		string ip = uLink.Network.player.ipAddress;
		print (serversManager);
		serversManager.RegisterServer (ip, listenPort, gameName);
		//}

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