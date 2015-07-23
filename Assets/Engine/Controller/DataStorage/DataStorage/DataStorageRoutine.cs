using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataStorageRoutine<ItemType> where ItemType: DataItem, new() {
	private int lastId = 0;
	
	public int GetNextId(){
		return lastId++;
	}
	
	public int PickId(int _id, Dictionary<int, ItemType> dataCollection){
		if( _id == -1 ){
			int result = GetNextId();
			while(dataCollection.ContainsKey ( result ))
				result = GetNextId();

			return result ;
		}else{
			if( dataCollection.ContainsKey ( _id ) ){
				Debug.LogError( "DataStorage Error: trying to pick used id : " + _id );
			}
			
			return _id;
		}
	}

	public string[] GetItemsUpdates( Dictionary<int, ItemType> dataCollection ){
		List<string> itemStrings = new List<string> ();
		foreach( KeyValuePair<int, ItemType> pair in dataCollection ){
			ItemType item = pair.Value;
			int id = pair.Key;
			
			if( item.HasUpdates() )
				itemStrings.Add( Serializer.Serialize( Separators.DataStorageActions , DataStorage<ItemType>.ItemActionUpdate , id, item.SerializeUpdates() ) );		
		}
		
		return itemStrings.ToArray ();
	}
	
	public string[] ConcatArrays(string[] array1, string[] array2){
		string[] result = new string[ array1.Length + array2.Length ];
		
		array1.CopyTo( result , 0 );
		array2.CopyTo( result , array1.Length );
		
		return result;
	}

	public string SerializeAddedItem( int id, ItemType item){
		return Serializer.Serialize( Separators.DataStorageActions , DataStorage<ItemType>.ItemActionAdd , id , item.SerializeWithController() );
	}
}
