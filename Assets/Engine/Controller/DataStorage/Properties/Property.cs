using UnityEngine;
using System.Collections;

public class Property {
	private object value = "";
	public bool obligatory = false;
	public bool controlledByServer = false;

	public Property(bool obligatory, bool controlledByServer){
		this.obligatory = obligatory;
		this.controlledByServer = controlledByServer;
	}
	
	public void Set(object value){		
		this.value = value; 
	}
	public virtual string Get(){ 
		return value.ToString (); 
	}
}
