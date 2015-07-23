using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net;

public class ServerGUI : MonoBehaviour {
	
	private string serverPort 	= "7000";
	private string maxPlayers 	= "32";
	private string gameName	  	= "game name";
	
	private bool readyToStartGame = false;
	
	public ServerLogic logic;
	public bool showUi;

	void OnGUI(){
		if( !showUi ){
			return;
		}

		Rect theRect = new Rect(350, 250, 300, 120);
		
		Rect centeredRect = CenterRectangle(theRect);
		
		if( uLink.Network.peerType != uLink.NetworkPeerType.Disconnected ){
			int width = 250;
			Rect rect = new Rect(Screen.width - width, 0, width, 150);
			GUI.Window (0, rect, InGameServerWindowContent, "Server Info");
		} else {
			GUI.Window (0, centeredRect, StartingServerWindowContent, "Server Config");
		}
	}
	
	private void InGameServerWindowContent( int winId ){
		if( GUILayout.Button( "SendData" ) ){
			//logic.SendData( "test data" );
			Messenger.Broadcast<string> ( "Network:sendData" , "test" );
		}

		CurrentHostAndPortInfo();
		ShowConnectedPlayers();
		KillServerButton();
	}
	
	private void StartingServerWindowContent( int winId ){
		readyToStartGame = true;
		ServerPort();
		MaxPlayers();
		GameName();
		StartServerButton();
	}
	
	// ------------------- GUI ELEMENTS ---------------------

	private void CurrentHostAndPortInfo(){
		GUILayout.Label( "port: " + serverPort);
		GUILayout.Label( "max players: " + maxPlayers );
	}

	private void ShowConnectedPlayers(){
		GUILayout.Label ( "Players : " + uLink.Network.connections.Length.ToString() );
	}
	
	private void ServerPort( ){
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Server Port" );
		
		serverPort = GUILayout.TextField( serverPort , 5 , GUILayout.Width( 100 ) );
		Regex rgx = new Regex("[^0-9]");
		serverPort = rgx.Replace(serverPort, "");
		
		if( serverPort == "" )
			readyToStartGame = false;
		
		GUILayout.EndHorizontal();
	}
	
	private void MaxPlayers( ){
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Max Players" );
		
		maxPlayers = GUILayout.TextField( maxPlayers , 2 , GUILayout.Width( 100 ) );
		Regex rgx = new Regex("[^0-9]");
		maxPlayers = rgx.Replace(maxPlayers, "");
		
		if( maxPlayers == "" )
			readyToStartGame = false;
		
		GUILayout.EndHorizontal();
	}
	
	private void GameName( ){
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Game Name" );
		
		gameName = GUILayout.TextField( gameName , 10 , GUILayout.Width( 100 ) );
		Regex rgx = new Regex("[^a-zA-Z0-9]");
		gameName = rgx.Replace(gameName, "");
		
		if( gameName == "" )
			readyToStartGame = false;
		
		GUILayout.EndHorizontal();
	}
	
	private void StartServerButton(){
		GUI.enabled = readyToStartGame;
		if( GUILayout.Button( "Start Server" ) ){
			int _maxPlayers = int.Parse( maxPlayers );
			int _serverPort = int.Parse( serverPort );

			logic.StartServer( _maxPlayers, _serverPort, gameName );
		}
		GUI.enabled = true;
	}
	
	private void KillServerButton(){
		if( GUILayout.Button( "Kill Server" ) ){
			logic.KillServer();
		}
	}
	
	// ------------------- ROUTINE --------------------------
	
	private Rect CenterRectangle ( Rect someRect ){
		someRect.x = ( Screen.width  - someRect.width ) / 2;
		someRect.y = ( Screen.height - someRect.height ) / 2;
		
		return someRect;
	}
}