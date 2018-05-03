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
			//Right colour
			if (piece.isActive && piece.type == type)
			{
				//Moving down
				if (piece.type == Piece.Type.black && !piece.isKing)
				{
					//Check if cell is occupied BL
					if (piece.cell.bottomLeft != null)
					{
						if (!(piece.cell.bottomLeft.piece == null))
						{
							if (piece.cell.bottomLeft.bottomLeft != null && piece.cell.bottomLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.bottomLeft.bottomLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.bottomLeft.bottomLeft, true);
									validMoves.Add(move);
								}
							}
						}
						//Not occupied so can move
						else
						{
							Move move = new Move(piece, piece.cell.bottomLeft);
							validMoves.Add(move);
						}
					}
					//Check if cell is occupied BR
					if (piece.cell.bottomRight != null)
					{
						if (!(piece.cell.bottomRight.piece == null))
						{
							if (piece.cell.bottomRight.bottomRight != null && piece.cell.bottomRight.piece.type != piece.type)
							{
								if (piece.cell.bottomRight.bottomRight.piece == null)
								{
									Move move = new Move(piece, piece.cell.bottomRight.bottomRight, true);
									validMoves.Add(move);
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.bottomRight);
							validMoves.Add(move2);
						}
					}
				}
				//Moving up
				else if (piece.type == Piece.Type.white && !piece.isKing)
				{
					if (piece.cell.topLeft != null)
					{
						if (!(piece.cell.topLeft.piece == null))
						{
							if (piece.cell.topLeft.topLeft != null && piece.cell.topLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.topLeft.topLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.topLeft.topLeft, true);
									validMoves.Add(move);
								}
							}
						}
						//Not occupied so can move
						else
						{
							Move move = new Move(piece, piece.cell.topLeft);
							validMoves.Add(move);
						}
					}
					//Check if cell is occupied TR
					if (piece.cell.topRight != null)
					{
						if (!(piece.cell.topRight.piece == null))
						{
							if (piece.cell.topRight.topRight != null && piece.cell.topRight.piece.type != piece.type)
							{
								if (piece.cell.topRight.topRight.piece == null)
								{
									Move move = new Move(piece, piece.cell.topRight.topRight, true);
									validMoves.Add(move);
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.topRight);
							validMoves.Add(move2);
						}
					}
				}
				//King movement
				else if (piece.isKing)
				{
					bool tlFilled = false;
					bool trFilled = false;
					bool blFilled = false;
					bool brFilled = false;
					foreach (Piece pieceOcc in pieces)
					{
						if (pieceOcc.cell == piece.cell.topLeft)
						{
							tlFilled = true;
						}
						if (pieceOcc.cell == piece.cell.topRight)
						{
							trFilled = true;
						}
					}
					if (!tlFilled)
					{
						Move move = new Move(piece, piece.cell.topLeft);
						validMoves.Add(move);
					}
					if (!trFilled)
					{
						Move move2 = new Move(piece, piece.cell.topRight);
						validMoves.Add(move2);
					}
					foreach (Piece pieceOcc in pieces)
					{
						if (pieceOcc.cell == piece.cell.bottomLeft)
						{
							blFilled = true;
						}
						if (pieceOcc.cell == piece.cell.bottomRight)
						{
							brFilled = true;
						}
					}
					if (!blFilled)
					{
						Move move3 = new Move(piece, piece.cell.bottomLeft);
						validMoves.Add(move3);
					}
					if (!brFilled)
					{
						Move move4 = new Move(piece, piece.cell.bottomRight);
						validMoves.Add(move4);
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
				turnText.text = turn == Turn.black ? "Turn: Black" : "Turn: White";
				return;
			}
		}
		Debug.Log("Not in the list of valid moves");
	}

	void gameOver()
	{
	}
}
