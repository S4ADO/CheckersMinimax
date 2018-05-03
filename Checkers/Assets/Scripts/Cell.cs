using UnityEngine;

public class Cell : MonoBehaviour
{
	//Declare cell properties
	public enum SpecialPosition {edge, corner}
	public SpecialPosition specialPosition;
	public int row, col;
	public Cell topLeft, topRight, bottomLeft, bottomRight;
	public MainGame mainGame;
	public Piece piece = null;

	//Init
	void Start()
	{
		Piece[] pieces = FindObjectsOfType<Piece>();
		foreach (Piece piece in pieces)
		{
			if (!piece.tag.Equals("clone"))
			{
				if (piece.cell == this)
				{
					this.piece = piece;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	//For humans playing
	void OnMouseDown()
	{
		if (mainGame.selectedPiece != null && piece == null)
		{
			mainGame.selectedCell = this;
			mainGame.makeMove();
		}
	}
}
