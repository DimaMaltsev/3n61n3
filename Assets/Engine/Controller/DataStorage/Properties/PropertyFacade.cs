using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PropertyFacade {
	private Dictionary<string,Property> properties = new Dictionary<string, Property>();

	protected virtual void OnNetworkUpdate(string variableName, object newValue){}

	private Property FindProperty( string name ){
		if (properties.ContainsKey (name)) {
			return properties[ name ];
		}else{
			return null;
		}
	}
	
	public void AddProperty( string name , bool obligatory ){		
		if ( properties.ContainsKey (name)) {
			Debug.Log( "Trying to add already existing property '" + name + "'" );
			return;
		}
		if( !properties.ContainsKey( name ) ){
			properties.Add (name, new Property (obligatory) );
		}
	}
	
	public float GetPropertyNumber( string name ){
		Property p = FindProperty( name );
		float result = 0;
		float.TryParse( p.Get() , out result );
		
		return result;
	}
	
	public string GetPropertyString( string name ){
		Property p = FindProperty( name );
		return p.Get();
	}
	
	public bool GetPropertyBoolean( string name ){
		Property p = FindProperty( name );
		bool result = false;
		bool.TryParse( p.Get() , out result );
		
		return result;
	}

	public bool IsObligatoryProperty(string name){
		return properties [name].obligatory;
	}
	
	public void _SetProperty( string name , object value ){
		Property p = FindProperty( name );
		p.Set( value );
	}

	public List<string> GetObligatoryProperties(){
		List<string> result = new List<string> ();
		foreach (KeyValuePair<string, Property> pair in properties)
			if (pair.Value.obligatory) 
				result.Add (pair.Key);
		return result;
	}

	// serializers

	protected string Serialize(List<string> propertyNames = null){
		if( propertyNames == null )
			return Serialize(properties);

		Dictionary<string, Property> result = new Dictionary<string, Property> ();

		propertyNames.ForEach( delegate(string propertyName) {
			if(result.ContainsKey(propertyName))
				result[propertyName] = FindProperty(propertyName);
			else
				result.Add(propertyName , FindProperty(propertyName ));
		});

		return Serialize( result );
	}

	private string Serialize(Dictionary<string, Property> properties){
		List<string> propertiesSerialized = new List<string>();
		foreach (KeyValuePair<string, Property> pair in properties)
			propertiesSerialized.Add ( Serializer.Serialize ( Separators.DataItemPropertyValue, pair.Key, GetPropertyString (pair.Key)) );
		
		return Serializer.SerializeArray(Separators.DataItemProperties , propertiesSerialized.ToArray() );
	}

	public virtual void Deserialize(string propertiesString){
		string[] propertyStrings = Serializer.Deserialize( Separators.DataItemProperties , propertiesString );
		for( int i = 0 ; i < propertyStrings.Length ; i++ ){
			string[] propertyDeserialized = Serializer.Deserialize( Separators.DataItemPropertyValue, propertyStrings[ i ] , false );
			string propertyName = propertyDeserialized[ 0 ];
			string propertyValue = propertyDeserialized[ 1 ] ;
			
			_SetProperty( propertyName , propertyValue );

			OnNetworkUpdate( propertyName, propertyValue);
		}
	}
}
