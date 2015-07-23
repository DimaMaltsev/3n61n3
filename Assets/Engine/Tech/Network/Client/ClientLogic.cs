using UnityEngine;
using System.Collections;

public class ClientLogic : AbstractNetworkLogic {
	protected override void Awake(){
		base.Awake ();

		new ClientNetworkEventer( this );
		Messenger.AddListener<string, int> (Events.Client.ConnectTo, Connect);
		Messenger.AddListener (Events.Client.Disconnect, Disconnect);
	}

	public void Connect( string serverAdress, int serverPort ){
		uLink.Network.Connect( serverAdress , serverPort );
	}
	
	public void Disconnect(){
		uLink.Network.Disconnect ();
	}

	void uLink_OnDisconnectedFromServer(){
		Messenger.Broadcast (Events.Network.DisconnectedFromServer);
	}
}