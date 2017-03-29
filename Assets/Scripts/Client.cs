using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour {

	public string clientName;
	public bool isHost;

	private bool socketReady; 
	private TcpClient socket;
	private NetworkStream stream;
	private StreamWriter writer;
	private StreamReader reader; 

	private List<GameClient> players = new List<GameClient>(); 

	//Connecting to the server

	private void Start() {
		DontDestroyOnLoad (gameObject);
	}

	public bool ConnectToServer(string host, int port) {
		if (socketReady)
			return false;

		try {
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);

			socketReady = true;
		}
		catch (Exception e){
			Debug.Log ("Socket error " + e.Message);
		}

		return socketReady;
	}

	private void Update() {
		if (socketReady) {
			if (stream.DataAvailable) {
				string data = reader.ReadLine ();
				if (data != null)
					OnIncomingData (data);
			}
		}
	}

	//Sending messages to server
	public void Send(string data)
	{
		if (!socketReady)
			return; 

		writer.WriteLine (data);
		writer.Flush ();
	}

	//Reading messages from server
	private void OnIncomingData(string data)
	{
		Debug.Log ("Client:" + data);

		string[] aData = data.Split ('|');

		switch (aData [0]) 
		{
		//WHO is at index 0, so start at index 1
		//if WHO messages gets sent, it is not the host
		case "WHO":
			for (int i = 1; i < aData.Length - 1; i++) {
				//receive info about who connected

				UserConnected (aData [i], false);

			}
			//"this is who i am"

			Send("CL|" + clientName + "|" + ((isHost)?1:0).ToString()); 
			break;
			//someone has connected
		case "SCNN":
			UserConnected (aData[1],false);
			break;

		}
	}

	public void UserConnected(string name, bool host) {
		GameClient c = new GameClient ();
		c.name = name; 

		players.Add(c);

		if (players.Count == 2)
			GameManager.Instance.StartGame();
	}

}

public class GameClient {

	public string name; 
	public bool isHost;


}