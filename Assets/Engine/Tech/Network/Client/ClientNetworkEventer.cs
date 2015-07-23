using UnityEngine;
using System.Collections;

public class ClientNetworkEventer : NetworkEventer {

	public ClientNetworkEventer( AbstractNetworkLogic logic ) : base ( logic ){
		Messenger.AddListener<string, string>( Events.Network.SendData , OnSendData );
	}

	private void OnSendData( string eventName , string data ){
		logic._SendData( uLink.NetworkPlayer.server , eventName , data );
	}
}
