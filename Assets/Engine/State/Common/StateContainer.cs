using UnityEngine;
using System.Collections;

public class StateContainer : MonoBehaviour {
	public void DestroySelf(){
		Destroy (gameObject);
	}
	
	void Awake(){
		if(!CoreFound())
			LoadCore();
	}
	
	private bool CoreFound(){
		return GameObject.FindGameObjectWithTag ("Core") != null;
	}
	
	private void LoadCore(){
		Messenger.Cleanup ();
		Application.LoadLevel( "core" );
	}
}
