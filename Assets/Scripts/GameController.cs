using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (playerController.turnSwitch == 1) {
			//print ("P1 turn");
		} else {
			//print ("P2 turn");
		}
	}
}
