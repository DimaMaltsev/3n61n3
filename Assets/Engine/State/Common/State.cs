using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class State : MonoBehaviour {
	private StateContainer container;

	protected string stateName;

	public virtual string GetData(){ return ""; }
	public virtual string GetSaveableStateData(){return "";}
	protected virtual void RegisterState(){
		Messenger.Broadcast (Events.State.RegisterState, this);
	}

	public virtual void ApplyInitData(string initData){}

	public void DestroySelf(){
		container.DestroySelf ();
	}

	public virtual void Awake(){
		RegisterState ();
		container = transform.parent.GetComponent<StateContainer> ();
	}

	public void SetName(string name){
		this.stateName = name;
	}

	public void WriteStateDataToCrossStorage(){
		CrossStageData.Set (CrossStageEntries.stateInitDataPrefix + stateName, GetSaveableStateData ());
	}

	public string ReadStateDataFromCrossStorage(){
		object initData = CrossStageData.Get (CrossStageEntries.stateInitDataPrefix + stateName);
		return initData == null ? "" : initData.ToString ();
	}

}
