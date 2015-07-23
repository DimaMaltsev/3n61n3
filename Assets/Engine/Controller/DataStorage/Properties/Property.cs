using UnityEngine;
using System.Collections;

public class Property {
	private object value = "";
	public bool obligatory = false;

	public Property(bool obligatory){
		this.obligatory = obligatory;
	}
	
	public void Set(object value){		
		this.value = value; 
	}
	public virtual string Get(){ 
		return value.ToString (); 
	}
}
