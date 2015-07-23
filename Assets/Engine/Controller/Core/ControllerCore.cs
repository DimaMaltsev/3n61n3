using UnityEngine;
using System.Collections;

public class ControllerCore<ItemType> where ItemType: DataItem, new() {	
	public DataStorage<ItemType> dataStorage = new DataStorage<ItemType>();

	private bool freezeUpdatesSend = false;

	protected Instantiator instantiator;

	public string GetUpdates(){
		if( freezeUpdatesSend )
			return "";

		string updates = dataStorage.GetUpdates ();
		if( updates != "" )
			dataStorage.ClearUpdatesCollections();

		return updates;
	}

	public void NetworkUpdate(string updateString, int updaterId = -1 ){
		dataStorage.ParseAndApplyUpdates (updateString , updaterId );
	}

	public void FreezeUpdatesSend(){
		freezeUpdatesSend = true;
	}

	public void FlushUpdates(){
		freezeUpdatesSend = false;
	}

	public virtual void Awake(){
		instantiator = GameObject.FindGameObjectWithTag ("Core").GetComponent<Instantiator> ();
	}

	public virtual void Update(){}
	public virtual void Enable(){}
	public virtual void Disable(){}
}
