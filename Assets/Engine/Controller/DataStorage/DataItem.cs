using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataItem : PropertyFacade {
	public List<string> updatedProperties = new List<string> ();
	public int controllerId = -1;
	public int id = -1;

	public bool controllable = false;

	protected virtual void Inititalize(){}
	public virtual void Update(){}
	public virtual void ServerUpdate(){}
	public virtual void ControllableUpdate(){}
	public virtual void RemoveGameObject(){}
	public virtual void NetworkUpdateHappened(){}
	public virtual void ControllableNetworkUpdateHappened (){}

	public DataItem(){}

	public DataItem( Dictionary<string,bool[]> properties ){
		foreach(KeyValuePair<string,bool[]> pair in properties ){
			AddProperty( pair.Key , pair.Value[0], pair.Value.Length > 1 ? pair.Value[1] : false );
		}
	}

	public void SetProperty( string name , object value ){
		_SetProperty( name , value );

		if (!IsObligatoryProperty (name)) {
			if( controllable || controllerId == -1) LogObligatoryProperties();
			LogPropertyUpdate (name);
		}
	}

	public string SerializeWithController(){
		return Serializer.Serialize( Separators.DataItemControllerProperties , controllerId , Serialize () );
	}
	
	public string SerializeUpdates(){
		return Serialize( updatedProperties );
	}

	public override void Deserialize (string propertiesString)
	{
		base.Deserialize (propertiesString);
		if (controllable)
			ControllableNetworkUpdateHappened ();
		else
			NetworkUpdateHappened ();
	}
	
	public void DeserializeWithController(string itemString){
		string[] data = Serializer.Deserialize( Separators.DataItemControllerProperties , itemString , false );
		
		int controllerId = int.Parse( data[ 0 ] );
		string propertiesString = data[ 1 ];

		this.controllerId = controllerId;
		controllable = CrossStageData.GetNumber (CrossStageEntries.myConnectionId) == controllerId;

		base.Deserialize( propertiesString );

		Inititalize ();
	}
	
	protected bool ignoreUpdate = false;
	
	public void IgnoreUpdates(){ ignoreUpdate = true; }
	public void UnignoreUpdates(){ ignoreUpdate = false; }

	public bool HasUpdates(){ return updatedProperties.Count != 0; }
	public void ClearUpdates(){ updatedProperties.Clear (); }

	public void LogObligatoryProperties(){
		if (ignoreUpdate) return;
		List<string> obligatoryProperties = GetObligatoryProperties ();
		
		for (int i = 0; i < obligatoryProperties.Count; i++) {
			string propertyName = obligatoryProperties [i];
			
			if(!updatedProperties.Contains(propertyName))
				updatedProperties.Add (propertyName);
		}
	}

	public void LogObligatoryServerProperties(){
		if (ignoreUpdate) return;
		List<string> obligatoryProperties = GetObligatoryServerProperties ();

		for (int i = 0; i < obligatoryProperties.Count; i++) {
			string propertyName = obligatoryProperties [i];

			if(!updatedProperties.Contains(propertyName))
				updatedProperties.Add (propertyName);
		}
	}

	private void LogPropertyUpdate(string name){
		if (ignoreUpdate) return;
		updatedProperties.Add( name );
	}
}
