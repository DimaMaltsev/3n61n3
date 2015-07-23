using UnityEngine;
using System.Collections;

public abstract class AbstractNetworkLogic : MonoBehaviour{
	protected uLink.NetworkView myNetworkView;
	
	public string 	masterServerIp;
	public int 		masterServerPort;

	protected virtual void Awake(){
		uLink.MasterServer.ipAddress 	= masterServerIp;
		uLink.MasterServer.port			= masterServerPort;
		
		myNetworkView = uLink.NetworkView.Get( this );
	}	

	public void _SendData( uLink.NetworkPlayer networkPlayer , string eventName , string data ){
		myNetworkView.RPC( "ReceiveData" , networkPlayer , Serializer.Serialize( Separators.NetworkEventData , eventName , data ) );
	}

	[RPC]
	protected void ReceiveData( string data, uLink.NetworkMessageInfo sender ){
		string[] deserializedData = Serializer.Deserialize( Separators.NetworkEventData , data , false );
		string eventName = deserializedData[ 0 ];
		string eventData = deserializedData[ 1 ];

		if( uLink.Network.isServer )
			Messenger.Broadcast( eventName , eventData , sender.sender.id );
		else
			Messenger.Broadcast( eventName , eventData );

	}
}
