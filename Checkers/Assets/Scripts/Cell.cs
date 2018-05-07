/**
 * This class describes the properties of each cell on a given board
 * where each board is made up for 32 cells
 * Author: Saad Musejee
 * */
using UnityEngine;

public class Cell : MonoBehaviour
{
	//Declare cell properties
	public enum SpecialPosition {none, edge}
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
