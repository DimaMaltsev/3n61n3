using UnityEngine;
using System.Collections;

public class StateMultiplayer : State {
	public float obligatoryPropertiesUpdateRate = 0;

	public virtual void BroadcastUpdates(string updates){}
	public virtual void UpdateObligatoryProperties(){}
	public virtual string GetUpdates(){ return ""; }
	public virtual string GetObligatoryUpdates(){ return ""; }
	public virtual void ApplyUpdates(string updates){}
	public virtual void ApplyUpdates(string updates, int updaterId){}
	public virtual void Resolved(){}

	public virtual bool CanAuthorize(){
		return true;
	}

	protected string SerializeUpdates(string separator, params string[] updates){
		for (int i = 0; i < updates.Length; i++)
			if(updates[i] != "")
				return Serializer.SerializeArray( separator, updates);

		return "";
	}

	protected override void RegisterState ()
	{
		Messenger.Broadcast (Events.State.RegisterState, this);
	}
	
	public virtual void Update(){
		string updates = GetUpdates ();
		if( updates == "" ) return;
		
		updates += GetObligatoryUpdates ();
		BroadcastUpdates (updates);
	}

	public virtual void Start(){
		if( obligatoryPropertiesUpdateRate != 0 )
			ObligatorySyncTick ();
	}

	private void ObligatorySyncTick(){
		UpdateObligatoryProperties ();
		Invoke ("ObligatorySyncTick", obligatoryPropertiesUpdateRate);
	}
}
