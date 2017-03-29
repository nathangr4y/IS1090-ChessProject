using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour {

	private Vector3 screenPoint;
	private Vector3	offset;

	public GameObject p1King;
	public GameObject p1Queen;
	public GameObject p1kBishop;
	public GameObject p1qBishop;
	public GameObject p1kKnight;
	public GameObject p1qKnight;
	public GameObject p1kRook;
	public GameObject p1qRook;
	public GameObject p1Pawn;

	public GameObject p2King;
	public GameObject p2Queen;
	public GameObject p2kBishop;
	public GameObject p2qBishop;
	public GameObject p2kKnight;
	public GameObject p2qKnight;
	public GameObject p2kRook;
	public GameObject p2qRook;
	public GameObject p2Pawn;

	public static int turnSwitch = 1;

	void Update () {
		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis ("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate (0, x, 0);
		transform.Translate (0, 0, z);
	}

	void OnMouseDown(){
		if (isLocalPlayer) {
			return;
		}
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);
		Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (cursorPoint);
	}

	void OnMouseDrag(){
		if(isLocalPlayer){
			return;
		}
			Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offset;
			transform.position = cursorPosition;
	}

	void OnMouseUp(){
		turnSwitch = -turnSwitch;
	}


	public override void OnStartLocalPlayer () {
		if (isClient && isServer) {
			CmdSpawnP1Pieces ();
		} else {
			CmdSpawnP2Pieces ();
		}
	}

	[Command]
	public void CmdSpawnP1Pieces()
	{
		p1King = (GameObject)Instantiate (p1King, new Vector3 (2.0f, 1.1f, 1.8f), transform.rotation);
		NetworkServer.Spawn (p1King);
		p1Queen = (GameObject)Instantiate (p1Queen, new Vector3 (0.0f, 1.1f, 1.7f), transform.rotation);
		NetworkServer.Spawn (p1Queen);
		p1kBishop = (GameObject)Instantiate (p1kBishop, new Vector3 (4.0f, 1.0f, 2.0f), transform.rotation);
		NetworkServer.Spawn (p1kBishop);
		p1qBishop = (GameObject)Instantiate (p1kBishop, new Vector3 (-2.0f, 1.0f, 2.0f), transform.rotation);
		NetworkServer.Spawn (p1qBishop);
		p1kKnight = (GameObject)Instantiate (p1kKnight, new Vector3 (6.0f, 1.0f, 2.0f), transform.rotation);
		NetworkServer.Spawn (p1kKnight);
		p1qKnight = (GameObject)Instantiate (p1kKnight, new Vector3 (-4.0f, 1.0f, 2.0f), transform.rotation);
		NetworkServer.Spawn (p1qKnight);
		p1kRook = (GameObject)Instantiate (p1kRook, new Vector3 (8.0f, 1.0f, 2.0f), transform.rotation);
		NetworkServer.Spawn (p1kRook);
		p1qRook = (GameObject)Instantiate (p1kRook, new Vector3 (-6.0f, 1.0f, 2.0f), transform.rotation);
		NetworkServer.Spawn(p1qRook);

		for (int i = -6; i <= 8; i += 2) {
			GameObject p1goldPawn = (GameObject)Instantiate (p1Pawn, new Vector3 (i, 1.1f, 4.0f), transform.rotation) as GameObject;
			NetworkServer.Spawn (p1goldPawn);
		}
	}

	[Command]
	public void CmdSpawnP2Pieces()
	{
		p2King = (GameObject)Instantiate (p2King, new Vector3(2.0f,1.1f,16.2f), transform.rotation);
		NetworkServer.Spawn (p2King);
		p2Queen = (GameObject)Instantiate (p2Queen, new Vector3(0.0f,1.1f,16.3f), transform.rotation);
		NetworkServer.Spawn (p2Queen);
		p2kBishop = (GameObject)Instantiate (p2kBishop, new Vector3(4.0f,1.0f,16.0f), transform.rotation);
		NetworkServer.Spawn (p2kBishop);
		p2qBishop = (GameObject)Instantiate (p2kBishop, new Vector3(-2.0f,1.0f,16.0f), transform.rotation);
		NetworkServer.Spawn (p2qBishop);
		p2kKnight = (GameObject)Instantiate (p2kKnight, new Vector3(-4.0f,1.0f,16.0f), transform.rotation);
		NetworkServer.Spawn (p2kKnight);
		p2qKnight = (GameObject)Instantiate (p2kKnight, new Vector3(6.0f,1.0f,16.0f), transform.rotation);
		NetworkServer.Spawn (p2qKnight);
		p2kRook = (GameObject)Instantiate (p2kRook, new Vector3(-6.0f,1.0f,16.0f), transform.rotation);
		NetworkServer.Spawn (p2kRook);
		p2qRook = (GameObject)Instantiate (p2kRook, new Vector3(8.0f,1.0f,16.0f), transform.rotation);
		NetworkServer.Spawn (p2qRook);

		for (int i = -6; i <= 8; i += 2) {
			GameObject p2silverPawn = (GameObject)Instantiate (p2Pawn, new Vector3 (i, 1.1f, 14.0f), transform.rotation) as GameObject;
			NetworkServer.Spawn (p2silverPawn);
		}
	}
}
