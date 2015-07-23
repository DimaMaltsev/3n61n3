using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour {
	protected string currentStateName = "";

	private State currentState;
	public string introStateName;

	public virtual void PreLoadState(string stateName){}

	protected virtual void Awake(){
		Messenger.AddListener<string>( Events.State.Load , OnLoadState );
		Messenger.AddListener (Events.Application.Configured, OnApplicationConfigured);
	}

	protected virtual void Start(){
		FixCoreEvents ();
	}

	protected void ChangeState(string stateName){
		PreLoadState (stateName);

		CleanUpLastState ();
		LoadNewState (stateName);
		currentStateName = stateName;
	}

	protected void SetCurrentState(State currentState){
		this.currentState = currentState;
	}

	// events

	private void OnLoadState(string stateName){
		ChangeState( stateName );
	}

	private void OnApplicationConfigured(){
		OnLoadState (introStateName);
	}

	protected void OnRegisterState(State state){
		currentState = state;
		state.SetName (currentStateName);
	}

	protected void FixCoreEvents(){
		List<string> events = new List<string>( Messenger.eventTable.Keys );
		
		for( int i = 0 ; i < events.Count ; i++ )
			Messenger.MarkAsPermanent( events[ i ] );
	}
	// privates
	private void CleanUpLastState(){
		if(currentState == null) return;

		Messenger.Cleanup ();
		currentState.DestroySelf ();
		currentState = null;
	}

	private void LoadNewState(string stateName){
		Application.LoadLevelAdditive (stateName);
	}
}
