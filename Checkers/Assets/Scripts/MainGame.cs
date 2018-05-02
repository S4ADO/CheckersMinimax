using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
	//UI to be used when game is over
	public StartGameUI UI;
	//Type of game (human vs AI / AI vs AI)
	public enum GameType {hvs, hvt, tvs}
	public GameType gameType;
	//Player turn
	public enum Turn {black, white}
	public Turn turn;
	public Text turnText;
	//Selected piece and cell
	public Cell selectedCell;
	public Piece selectedPiece;

	//Initialise the game state
	public void init(GameType type)
	{
		gameType = type;
		//Assign random first turn
		turn = (int)Time.time % 2 == 0 ? Turn.black : Turn.white;
		turnText.text = turn == Turn.black ? "Turn: Black" : "Turn: White";
		//Assign cells their adjacents
		Cell[] cells = FindObjectsOfType<Cell>();
		foreach (Cell cell in cells)
		{
			//This cell's IDs
			int cellRow = cell.row;
			int cellCol = cell.col;
			//Adjacent IDs
			int tlR, tlC, trR, trC, blR, blC, brR, brC;
			tlR = cellRow - 1;
			tlC = cellCol + 1;
			trR = cellRow + 1;
			trC = cellCol + 1;
			blR = cellRow - 1;
			blC = cellCol - 1;
			brR = cellRow + 1;
			brC = cellCol - 1;
			//Check if a cell with these IDs exists
			foreach (Cell cellCheck in cells)
			{
				//Assign top left
				if (cellCheck.row == tlR && cellCheck.col == tlC)
				{
					cell.topLeft = cellCheck;
				}
				//Assign top right
				else if (cellCheck.row == trR && cellCheck.col == trC)
				{
					cell.topRight = cellCheck;
				}
				//Assign bottom left
				else if (cellCheck.row == blR && cellCheck.col == blC)
				{
					cell.bottomLeft = cellCheck;
				}
				//Assign bottom right
				else if (cellCheck.row == brR && cellCheck.col == brC)
				{
					cell.bottomRight = cellCheck;
				}
			}
		}
	}

	//Get all valid moves for a given piece
	private List<Move> getAllValidMoves(Piece.Type type)
	{
		List<Move> validMoves = new List<Move>();
		Piece[] pieces = FindObjectsOfType<Piece>();
		foreach (Piece piece in pieces)
		{
			if (piece.isActive && piece.type == type)
			{
				//Check if cell is occupied
				foreach (Piece pieceOcc in pieces)
				{
					if (pieceOcc.cell == selectedCell)
					{
						Debug.Log("Piece already exists in that cell");
						return validMoves;
					}
				}
				//Moving down
				if (piece.type == Piece.Type.black && !piece.isKing)
				{
					if (selectedCell == piece.cell.bottomLeft || selectedCell == piece.cell.bottomRight)
					{
						Move move = new Move(selectedPiece, selectedCell);
						validMoves.Add(move);
					}
				}
				//Moving up
				else if (piece.type == Piece.Type.white && !piece.isKing)
				{
					if (selectedCell == piece.cell.topLeft || selectedCell == piece.cell.topRight)
					{
						Move move = new Move(selectedPiece, selectedCell);
						validMoves.Add(move);
					}
				}
				else if (piece.isKing)
				{
					if (selectedCell == piece.cell.topLeft || selectedCell == piece.cell.topRight || 
						selectedCell == piece.cell.bottomLeft || selectedCell == piece.cell.bottomRight)
					{
						Move move = new Move(selectedPiece, selectedCell);
						validMoves.Add(move);
					}
				}
			}
		}
		return validMoves;
	}

	//Make the move
	public void makeMove()
	{
		List<Move> validMoves = new List<Move>();
		if (turn == Turn.black)
		{
			validMoves = getAllValidMoves(Piece.Type.black);
		}
		else if (turn == Turn.white)
		{
			validMoves = getAllValidMoves(Piece.Type.white);
		}
		Move currentMove = new Move(selectedPiece, selectedCell);
		foreach (Move move in validMoves)
		{
			if (move.getCell() == selectedCell && move.getPiece() == selectedPiece)
			{
				selectedPiece.movePiece(selectedCell);
				//Make king if cell to move to is end row
				selectedCell = null;
				selectedPiece = null;
				turn = turn == Turn.black ? Turn.white : Turn.black;
				return;
			}
		}
		Debug.Log("Not in the list of valid moves");
	}

	void gameOver()
	{
	}
}
