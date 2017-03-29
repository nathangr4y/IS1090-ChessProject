using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLog : NetworkBehaviour {

	public List<string> EventLog = new List<string>();
	private string guiText = "";
	public int count;

	public void OnGUI()
	{
		GUI.Label (new Rect (0, Screen.height - (Screen.height / 3), Screen.width / 4, Screen.height / 3), guiText, GUI.skin.textArea);
	}

	public void AddEvent(string eventString)
	{
		EventLog.Add(eventString);

		guiText = "";

		foreach (string logEvent in EventLog)
		{
			guiText += logEvent;
			guiText += "\n";
		}
	}


	// Use this for initialization
	void Start () {
		EventLog = GetComponent<List<string>> ();

	}

	// Update is called once per frame
	public void Update () {

		count = EventLog.Count;

		if (Input.GetMouseButtonDown (0)) {

			if (isClient && isServer) {
				AddEvent (count.ToString () + ". ");
				AddEvent ("White move: " + BoardController.x +" "+BoardController.y);
				AddEvent (GameObject.Find("BoardController").GetComponent<BoardController>().endDrag.ToString());

			}
		} 
		if (Input.GetMouseButtonDown (0)) {
			if(isClient) {
				AddEvent (count.ToString () + ". ");
				AddEvent ("Black move: " + GameObject.Find ("BoardController").GetComponent<BoardController> ().startDrag.ToString());
				AddEvent (GameObject.Find("BoardController").GetComponent<BoardController>().endDrag.ToString());
			}
		}
	}

	public void getSquare(int x, int y){
		if (x == -6) {
		}
	}
}