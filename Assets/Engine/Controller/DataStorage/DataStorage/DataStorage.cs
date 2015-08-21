using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataStorage<ItemType> where ItemType: DataItem, new() {
	private float myConnectionId = -2;

	public List<string> storageUpdates = new List<string> ();
	public DataStorageRoutine<ItemType> routine = new DataStorageRoutine<ItemType>();
	public Dictionary<int, ItemType> dataCollection = new Dictionary<int, ItemType>();

	private bool ignoreUpdates = false;

	public void Update(){
		if (myConnectionId == -2) // TODO: fix this -2 check
			myConnectionId = CrossStageData.GetNumber(CrossStageEntries.myConnectionId);

		foreach (KeyValuePair<int, ItemType> pair in dataCollection) {
			if( myConnectionId == pair.Value.controllerId )
				pair.Value.ControllableUpdate();

			if( uLink.Network.isServer )
				pair.Value.ServerUpdate();

			pair.Value.Update ();
		}
	}

	public int AddItem(ItemType item, int _id = -1){

		int id = routine.PickId( _id , dataCollection );

		dataCollection.Add (id, item);
		RegisterUpdate( routine.SerializeAddedItem( id , item ) );
		item.id = id;

		return id;
	}

	public void RemoveItem(int id){
		if( !dataCollection.ContainsKey( id ) ) Debug.LogError( "DataStorage Error: removing not existing item : " + id );
		dataCollection [id].RemoveGameObject ();
		dataCollection.Remove( id );
		RegisterUpdate( Serializer.Serialize( Separators.DataStorageActions , ItemActionRemove , id ) );
	}

	public ItemType FindItemByController(int id){
		foreach (KeyValuePair<int, ItemType> pair in dataCollection) {
			if( pair.Value.controllerId == id ){
				return pair.Value;
			}
		}
		Debug.LogError( "DataStorage Error: controller does not controll any items : " + id );
		return null;
	}

	public string GetUpdates(){
		string[] itemsUpdates = routine.GetItemsUpdates (dataCollection);
		return SerializeUpdates( routine.ConcatArrays( itemsUpdates , storageUpdates.ToArray() ) );
	}

	public void ClearUpdatesCollections(){
		storageUpdates.Clear();
		foreach( KeyValuePair<int, ItemType> pair in dataCollection )
			pair.Value.ClearUpdates();
	}

	private string SerializeUpdates( string[] updates ){
		return Serializer.SerializeArray( Separators.DataStorageUpdate , updates );
	}

	private string[] DeserializeUpdates( string updatesString ){
		return Serializer.Deserialize( Separators.DataStorageUpdate , updatesString );
	}

	private void RegisterUpdate( string update ){
		if( !ignoreUpdates )
			storageUpdates.Add( update );
	}
	
	public string CreateAddItemsCommand(){
		List<string> itemsStrings = new List<string> ();
		foreach( KeyValuePair<int, ItemType> pair in dataCollection )
			itemsStrings.Add( routine.SerializeAddedItem( pair.Key , pair.Value ) );		

		return SerializeUpdates( itemsStrings.ToArray() );
	}

	public void IgnoreUpdates(){		
		ignoreUpdates = true;
	}

	public void UnignoreUpdates(){		
		ignoreUpdates = false;
	}

	public void LogObligatoryProperties(){
		int myConnectionId = Mathf.FloorToInt(CrossStageData.GetNumber (CrossStageEntries.myConnectionId));
		foreach (KeyValuePair<int, ItemType> pair in dataCollection) {
			if( pair.Value.controllerId == myConnectionId )
				pair.Value.LogObligatoryProperties ();
		}
	}

	public void LogServerObligatoryProperties(){
		foreach (KeyValuePair<int, ItemType> pair in dataCollection) {
			pair.Value.LogObligatoryServerProperties();
		}
	}

	// network actions
	
	public static string ItemActionAdd = "A";
	public static string ItemActionRemove = "R";
	public static string ItemActionUpdate = "U";
	
	public void ParseAndApplyUpdates(string updatesString , int updaterId ){
		IgnoreUpdates ();
		
		string[] updateStrings = DeserializeUpdates( updatesString );
		for( int i = 0 ; i < updateStrings.Length ; i++ )
			ParseAndApplyUpdateString( updateStrings[ i ] , updaterId );
		
		UnignoreUpdates ();
	}

	private void ParseAndApplyUpdateString( string updateString, int updaterId ){
		string[] actionString = Serializer.Deserialize( Separators.DataStorageActions , updateString );
		
		string action = actionString[ 0 ];
		int id = int.Parse( actionString[ 1 ] );
		string actionData = actionString.Length > 2 ? actionString[ 2 ] : "";
		
		HandleUpdate( action , id , actionData );
	}
	
	private void HandleUpdate( string action , int id , string updateString ){
		if( action == ItemActionAdd ){ // ADD ITEM
			ItemType itemToAdd = new ItemType();
			itemToAdd.DeserializeWithController( updateString  );
			AddItem( itemToAdd , id );
		}
			
		if( action == ItemActionRemove ) // REMOVE ITEM
			RemoveItem( id );

		if( action == ItemActionUpdate ) // Update item
			dataCollection[ id ].Deserialize( updateString );
	}
}