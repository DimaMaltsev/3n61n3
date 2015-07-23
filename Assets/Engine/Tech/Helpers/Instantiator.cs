using UnityEngine;
using System.Collections;

public class Instantiator : MonoBehaviour {
	public GameObject GetObject(string objectPath){ // TODO: make it cash objects
		return (GameObject)Instantiate(Resources.Load("Prefabs/" + objectPath));
	}
}
