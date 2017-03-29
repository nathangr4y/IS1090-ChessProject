using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.IO;

public class Server : MonoBehaviour {

	public int port = 6123;

	private List<ServerClient> clients;
	private List<ServerClient> disconnected; 

	private TcpListener server; 
	private bool serverStarted;

	public void Init() {
		DontDestroyOnLoad (gameObject); 
		clients = new List<ServerClient> ();
		disconnected = new List<ServerClient> ();

		try {
			server = new TcpListener(IPAddress.Any, port);
			server.Start(); 

			StartListening();
			serverStarted = true;
		}
		catch(Exception e) {
			Debug.Log("Socket error: " + e.Message);
		}

	}

	private void Update() {
		if (!serverStarted)
			return;

		foreach (ServerClient c in clients) {
			//is client connected?

			if (!IsConnected (c.tcp)) {

				c.tcp.Close ();
				disconnected.Add (c);
				continue;
			} else {

				NetworkStream s = c.tcp.GetStream ();
				if (s.DataAvailable) {

					StreamReader reader = new StreamReader (s, true);
					string data = reader.ReadLine(); 

					if (data != null) {	 
						OnIncomingData (c, data);
					}
				}
			}
		}

		for (int i = 0; i < disconnected.Count - 1; i++) {

			// someone has disconnected 

			clients.Remove (disconnected [i]);
			disconnected.RemoveAt (i);

		}
	}

	private void StartListening() {
		server.BeginAcceptTcpClient(AcceptTcpClient, server);
	}

	private void AcceptTcpClient(IAsyncResult ar) {
		TcpListener listener = (TcpListener)ar.AsyncState;

		//creating list of users
		string allUsers = "";
		foreach(ServerClient i in clients) {

			allUsers += i.clientName + '|';
		}

		ServerClient sc = new ServerClient (listener.EndAcceptTcpClient (ar));
		clients.Add(sc); 

		StartListening ();

		Debug.Log ("Someone has connected!");

		//sending message

		Broadcast ("WHO|" + allUsers, clients [clients.Count - 1]);

	}

	private void Broadcast(string data, ServerClient c) {

		List<ServerClient> sc = new List<ServerClient> { c };
		Broadcast (data, sc); 

	}

	private bool IsConnected(TcpClient c){
		try{
			if(c != null & c.Client != null && c.Client.Connected)
			{
				byte[] buff = new byte[1];
				if(c.Client.Poll(0,SelectMode.SelectRead))
					return !(c.Client.Receive(buff, SocketFlags.Peek) == 0);

				return true;
			} 
			else {
				return false;
			}
		}
		catch {
			return false;
		}
	}

	private void Broadcast(string data, List<ServerClient> cl)
	{
		foreach (ServerClient sc in cl) {

			try {
				StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
				writer.WriteLine(data);
				writer.Flush();
			}
			catch (Exception e){
				Debug.Log ("write error : " + e.Message);
			}

		}
	}

	private void OnIncomingData (ServerClient c, string data){
		Debug.Log ("Server:" + data);

		string[] aData = data.Split ('|');

		switch (aData [0]) {
		//if client is sending
		case "WHO":
			c.clientName = aData [1];
			c.isHost = (aData [2] == "0") ? false : true;
			Broadcast ("SCNN|" + c.clientName, clients);
			break;
		}
	}


}

public class ServerClient {
	public string clientName;
	public TcpClient tcp; 
	public bool isHost;

	public ServerClient (TcpClient tcp)
	{
		this.tcp = tcp;
	}
}
