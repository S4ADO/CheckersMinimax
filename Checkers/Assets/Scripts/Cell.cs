using UnityEngine;

public class Cell : MonoBehaviour
{
	//Declare cell properties
	public enum SpecialPosition { edge, corner }
	public SpecialPosition specialPosition;
	public int row, col;
	public Cell topLeft, topRight, bottomLeft, bottomRight;
	public Board mainBoard;
	public Piece piece = null;

	//Init
	void Start()
	{
		Piece[] pieces = FindObjectsOfType<Piece>();
		foreach (Piece piece in pieces)
		{
			if (piece.cell == this)
			{
				this.piece = piece;
			}
		}
	}

	//For humans playing
	void OnMouseDown()
	{
		if (mainBoard.selectedPiece != null && piece == null)
		{
			mainBoard.selectedCell = this;
			mainBoard.makeMove();
		}
	}
}
