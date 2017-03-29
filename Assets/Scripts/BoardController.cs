//Knowm issues: 
// 1) All control of pieces goes to the Host. The client sees updates, but any changes made by the client are not sent to the server. 
// 2) The log shows starting coordinates, though not ending coordinates. 
// 3) Pieces are not destroyed. (This wasn't in the requirements, though we figured it was a given)
// Most code is ours though we did refer to the following tutorial: youtube.com/watch?v=-nLP0Qz81fE&index=1&list=PLLH3mUGkFCVXrGLRxfhst7pffE9o2SQO
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoardController : MonoBehaviour {

	public static float x, y, z;
	private const int OFFSET = 8;
	private List<GameObject> boardSquares;
	public GameObject boardSquareWhite;
	public GameObject boardSquareBlack;

	public Piece[,] pieces = new Piece[8,8];
	public GameObject goldPawn;
	public GameObject silverPawn;

	private bool isWhite;
	private bool isWhiteTurn;

	private Piece selectedPiece;

	public Vector2 mouseOver;
	public Vector2 startDrag;
	public Vector2 endDrag;

	public int oldTurnSwitch = -1;

	void Start () {
		isWhiteTurn = true;


		boardSquares = new List<GameObject> ();
		x = 0f;
		y = 1f;
		z = 0f;
		int row = 1;
		int col = 1;
		int switchStartingColor = 0;
		for (int i = 1; i <= 8; i++) {
			col = 1;
			for (int j = 1; j <= 8; j++) {
				x = (float) (row * 2- OFFSET);
				z = (float) (col * 2);

				GameObject boardSquareObj;
				if (col % 2 == switchStartingColor) {
					boardSquareObj = (GameObject)Instantiate (boardSquareWhite, new Vector3 (x, y, z), transform.rotation);	
				} else {
					boardSquareObj = (GameObject)Instantiate (boardSquareBlack, new Vector3 (x, y, z), transform.rotation);
				}

				boardSquares.Add (boardSquareObj);

				BoardSquare boardSquareScript = boardSquareObj.GetComponent<BoardSquare> ();
				//boardSquareScript.setCellNumber (row);
				//boardSquareScript.setCellLetter ((char) (col + 96));
				col = col + 1;
			}
			if (switchStartingColor == 0) {
				switchStartingColor = 1;
			} else {
				switchStartingColor = 0; 	}
			row = row + 1;
		}
	}
	void Update () {

		UpdateMouseOver();

		//If it is my turn
		{
			int x = (int)mouseOver.x;
			int y = (int)mouseOver.y;

			if(selectedPiece != null)
				UpdatePieceDrag(selectedPiece);

			if (Input.GetMouseButtonDown (0))
				SelectPiece (x, y);
			
			if (Input.GetMouseButtonUp (0))
				TryMove ((int)startDrag.x, (int)startDrag.y, x, y);
		}

		Debug.Log (mouseOver);

		if (playerController.turnSwitch != oldTurnSwitch){
			Camera.main.transform.Rotate (0.0f, 0.0f, 180.0f);
			oldTurnSwitch = playerController.turnSwitch;
		}
	}

	private void UpdateMouseOver()
	{
		if (!Camera.main) {
			Debug.Log ("unable to find main camera");
			return;
		}

		RaycastHit hit; 
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("Board"))) {
			mouseOver.x = (int)hit.point.x;
			mouseOver.y = (int)hit.point.z;
		} else {
			mouseOver.x = -1;
			mouseOver.y = -1;
		}
	}
	private void UpdatePieceDrag(Piece p)
	{
		if (!Camera.main) {
			Debug.Log ("unable to find main camera");
			return;
		}

		RaycastHit hit; 
		if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 25.0f, LayerMask.GetMask ("Board"))) {
			p.transform.position = hit.point + Vector3.up;
		}
	}

	private void SelectPiece(int x, int y)
	{
		//out of bounds
		if(x<-6.0f || x>=8.0f || y<1.0f || y>=16.0f){
			return;
		}
		Piece p = pieces[x,y];
		if(p != null)
		{
			selectedPiece = p;
			startDrag = mouseOver;
		}

	}

	private void TryMove(int x1, int y1, int x2, int y2)
	{
		startDrag = new Vector2(x1,y1);
		endDrag = new Vector2(x2,y2);
		selectedPiece = pieces[x1,y1];

		//Check if OOB
		if (x < -6.0f || x >= 8.0f || y < 1.0f || y >= 16.0f) {
			if (selectedPiece != null)
				//MovePiece (selectedPiece, x1,y1);
			startDrag = Vector2.zero;
			selectedPiece = null;
			return;
		}

		if (selectedPiece != null) {
			//if it has not moved
			if (endDrag == startDrag) {
				//MovePiece (selectedPiece, x1, y1);
				startDrag = Vector2.zero;
				selectedPiece = null;
				return;
			}
		}

		if (selectedPiece.ValidMove (pieces, x1, y1, x2, y2)) {
			if (Mathf.Abs (x1 - x2) == 2) {
				Piece p = pieces [(x1 + x2) / 2, (y1 + y2) / 2];
				if (p != null) {
					pieces [(x1 + x2) / 2, (y1 + y2) / 2] = null;
					Destroy (p);
				}
			}
			
			pieces [x2, y2] = selectedPiece;
			pieces [x1, y1] = null;

		} else {
			startDrag = Vector2.zero;
			selectedPiece = null;
			return;
		}
	}
}
