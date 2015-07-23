using UnityEngine;
using System.Collections;

public class NetworkEventer {
	protected AbstractNetworkLogic logic;

	public NetworkEventer( AbstractNetworkLogic logic ){
		this.logic = logic;
	}
}
