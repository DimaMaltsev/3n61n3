using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Serializer {
	public static string SerializeArray(string separator, object[] pars){
		string result = "";
		for(int i = 0 ; i < pars.Length ; i++ ){
			result += pars[ i ].ToString() + ( i == pars.Length - 1 ? "" : GetSeparator( separator ) ) ;
		}
		
		return result;
	}
	
	public static string Serialize(string separater, params object[] pars){
		return SerializeArray( separater , pars );
	}
	
	public static string[] Deserialize(string incomingSeparator, string data, bool removeEmptyEntries = true ){
		string separator = GetSeparator( incomingSeparator );

		if( removeEmptyEntries )
			return data.Split( new string[]{ separator } , System.StringSplitOptions.RemoveEmptyEntries );
		else
			return data.Split( new string[] { separator } , System.StringSplitOptions.None );
	}

	private static string GetSeparator( string incomingSeparator ){
		return Separators.Main + incomingSeparator;
	}
}