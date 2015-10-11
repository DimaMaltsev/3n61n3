using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Events {
	public static class Network{		
		public static string StateInitData = "Network:stateInitData";
		public static string StateInitDataRequest = "Network:stateInitDataRequest";
		public static string DataStorageUpdate = "Network:dataStorageUpdate";
		public static string DataStorageUpdateWithSender = "Network:dataStorageUpdateWithSender";
		
		public static string AuthorizeRequest = "Network:authorizeRequest";
		public static string ChangeState = "Network:changeState";
		public static string ServerResolvedState = "Network:serverResolvedState";
		public static string ClientResolvedState = "Network:clientResolvedState";

		public static string ReceiveData = "Network:receiveData";
		public static string SendData = "Network:sendData";
		public static string Authorized = "Network:authorized";
		public static string DisconnectedFromServer = "Network:disconnectedFromServer";

		public static string SendAjax = "Network:sendAjax";
		public static string AjaxResponse = "Network:ajaxResponse";
	}

	public static class Server{
		public static string ForceClientDisconnect = "Server:forceClientDisconnect";
		public static string PlayerConnected = "Network:playerConnected";
		public static string PlayerDisconnected = "Network:playerDisconnected";
		public static string SendDataAll = "Network:sendDataAll";
		public static string SendDataExcept = "Network:sendDataExcept";
		public static string Authorized = "Network:authorized";
		public static string SentInitDataToClient = "Network:sentInitDataToClient";
		public static string RegisteredServer = "ServersManager:registeredServer";
		public static string UpdateServerInfo = "ServersManager:updateServerInfo";
	}

	public static class DataItem{
		public static string UpdateProperty = "DataItem:updateProperty";
	}

	public static class DataStorage{
		public static string HasUpdates = "DataStorage:hasUpdates";
	}

	public static class State{
		public static string Load = "State:load";
		public static string RegisterState = "State:registerState";
		public static string Launch = "State:launch";
		public static string ClientResolvedState = "State:clientResolved";
	}

	public static class Client{
		public static string ConnectTo = "Client:connectTo";
		public static string Disconnect = "Client:disconnect";
		public static string FetchServersList = "Client:fetchServersList";
		public static string ServersListUpdated = "Client:serversListUpdated";
	}

	public static class Application{
		public static string Configured = "Application:configured";
		public static string NetworkSplitterUpdate = "Application:networkSplitterUpdate";
	}
}
