using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class ClientGUI : MonoBehaviour {
	private string 	serverAdress 	= "127.0.0.1";
	private string 	serverPort		= "7000";

	private bool readyToConnect = false;

	public ClientLogic logic;
	public bool showUi;

	void OnGUI(){

		if (!showUi) {
			return;
		}

		switch( uLink.Network.peerType ){

			case uLink.NetworkPeerType.Disconnected:
				Rect centeredRect = CenterRectangle(new Rect(350, 250, 300, 70));
				GUI.Window (0, centeredRect, ManualServerConnect, "Direct connection (ip + port)");
			break;

			case uLink.NetworkPeerType.Client: 
				int width = 150;
				Rect rect = new Rect(Screen.width - width, 0, width, 120);
				GUI.Window (0, rect, GameInfoWindow, "Client Info");
			break;

		}
	}

	private void GameInfoWindow( int winId ){
		if( GUILayout.Button( "Disconnect" ) )
			logic.Disconnect();
	}
	
	private void ManualServerConnect( int winId ){
		readyToConnect = true;
		ServerData();
		ConnectButton();
	}
	
	// ------------------- UI ELEMENTS ------------------------------


	private void ServerData(){
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Server ip adress" );
		
		serverAdress = GUILayout.TextField( serverAdress , 15 );
		Regex rgx = new Regex("[^.0-9]");
		serverAdress = rgx.Replace(serverAdress, "");
		
		if( serverAdress == "" )
			readyToConnect = false;
		
		GUILayout.Label( "Server port" );
		
		serverPort = GUILayout.TextField( serverPort , 5 );
		rgx = new Regex("[^0-9]");
		serverPort = rgx.Replace(serverPort, "");
		
		if( serverPort == "" )
			readyToConnect = false;
		
		GUILayout.EndHorizontal();
	}
	
	private void ConnectButton(){
		GUI.enabled = readyToConnect;
		if( GUILayout.Button( "Connect To Server" ) ){
			logic.Connect( serverAdress, int.Parse( serverPort ) );
		}
		GUI.enabled = true;
	}

	// ------------------- ROUTINE --------------------------
	
	private Rect CenterRectangle ( Rect someRect ){
		someRect.x = ( Screen.width  - someRect.width ) / 2;
		someRect.y = ( Screen.height - someRect.height ) / 2;
		
		return someRect;
	}
}