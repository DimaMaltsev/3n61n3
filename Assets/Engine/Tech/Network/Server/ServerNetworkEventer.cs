using UnityEngine;
using System.Collections;

public class ServerNetworkEventer {
	ServerLogic logic;

	public ServerNetworkEventer( ServerLogic logic ) {

		this.logic = logic;

		Messenger.AddListener<int, string, string>( Events.Network.SendData , SendData );
		Messenger.AddListener<string, string>( Events.Server.SendDataAll , SendDataAll );
		Messenger.AddListener<int, string, string>( Events.Server.SendDataExcept , SendDataExcept );
	}

	public void SendData( int id , string eventName , string data ){
		logic._SendData( FindNetworkPlayer( id ) , eventName , data );
	}

	public void SendDataAll( string eventName , string data ){
		for( int i = 0 ; i < uLink.Network.connections.Length ; i++ ){
			int id = uLink.Network.connections[ i ].id;

			if(CanSendToThisConnection(id))
				logic._SendData( uLink.Network.connections[ i ] , eventName , data );
		}
	}

	public void SendDataExcept( int id , string eventName , string data ){
		for( int i = 0 ; i < uLink.Network.connections.Length ; i++ ){
			int _id = uLink.Network.connections[ i ].id;

			if( _id != id && CanSendToThisConnection(_id))
				logic._SendData( uLink.Network.connections[ i ] , eventName , data );
		}
	}

	private bool CanSendToThisConnection(int id){
		if(!logic.stateManager.connections.ContainsKey(id))
			return false;

		return logic.stateManager.connections [id].resolved;
	}

	private uLink.NetworkPlayer FindNetworkPlayer( int id ){
		for( int i = 0; i < uLink.Network.connections.Length ; i++ ){
			if( uLink.Network.connections[ i ].id == id )
				return uLink.Network.connections[ i ];
		}

		Debug.LogError( "Did not find a networkPlayer with id '" + id + "'" );
		return uLink.NetworkPlayer.unassigned;
	} 
}
