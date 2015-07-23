using UnityEngine;
using System.Collections;

public class ServerClientSplitter : MonoBehaviour {

	public GameObject ServerContent;
	public GameObject ClientContent;
	public GameObject SinglePlayerContent;
	private Config config;

	void Awake(){
		GameObject core = GameObject.FindGameObjectWithTag( "Core" );
		if( core == null )
			return;

		config = core.GetComponent<Config> ();
		UpdateSplitter ();
	}

	private void UpdateSplitter(){
		if (config == null) {
			Debug.LogError( "NetworkSplitter: config is not defined" );
			return;
		}

		bool server = uLink.Network.isServer || config.isServer;

		if( server )
			EnableServerContent();
		else
			EnableClientContent();
	}

	private void EnableSinglePlayerContent(){
		if( ServerContent != null ) ServerContent.SetActive (false);
		if( ClientContent != null ) ClientContent.SetActive (false);
		if( SinglePlayerContent != null ) SinglePlayerContent.SetActive (true);
	}

	private void EnableServerContent(){
		if( ClientContent != null ) ClientContent.SetActive (false);
		if( SinglePlayerContent != null ) SinglePlayerContent.SetActive (false);
		if( ServerContent != null ) ServerContent.SetActive (true);
	}

	private void EnableClientContent(){
		if( SinglePlayerContent != null ) SinglePlayerContent.SetActive (false);
		if( ServerContent != null ) ServerContent.SetActive (false);
		if( ClientContent != null ) ClientContent.SetActive (true);
	}
}
