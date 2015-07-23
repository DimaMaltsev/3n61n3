using UnityEngine;
using System.Collections;

public class UnityObjectNetworker : MonoBehaviour {

	public string registerEvent;

	public void RegisterSelf(){
		Messenger.Broadcast( registerEvent, gameObject );
	}

	public void RemoveSelf(){
		Destroy (gameObject);
	}
}
