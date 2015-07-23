using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CrossStageData {
	private static Dictionary<string, object> data = new Dictionary<string, object>();

	public static void Set(string name, object value){
		if ( !FieldExists( name ) ) {
			data.Add( name , value );
			return;
		}

		data[ name ] = value;
	}

	public static void Remove(string name){
		if( FieldExists( name ) ){
			data.Remove( name );
		}
	}

	public static float GetNumber( string name ){
		return FieldExists( name ) ? float.Parse( data[ name ].ToString() ) : 0;
	}

	public static string GetString( string name ){
		return FieldExists( name ) ? data[ name ].ToString() : "";
	}

	public static bool GetBoolean( string name ){
		return FieldExists( name ) ? bool.Parse( data[ name ].ToString() ) : false;
	}

	public static object Get( string name ){
		return FieldExists( name ) ? data[ name ] : null;
	}

	private static bool FieldExists(string name){
		return data.ContainsKey( name );
	}
}
