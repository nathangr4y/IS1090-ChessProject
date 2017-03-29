using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour {

	private int cellNumber;
	private char cellLetter;
	private string color;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCellNumber(int num){
		this.cellNumber = num;
	}

	public void setCellLetter(char letter){
		this.cellLetter = letter;
	}

	public void setColor(string color){
		this.color = color;
	}

}
