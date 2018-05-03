using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
	//UI to be used when game is over
	public StartGameUI UI;
	//Type of game (human vs AI / AI vs AI)
	public enum GameType { hvs, hvt, tvs }
	public GameType gameType;
	//Player turn
	public enum Turn { black, white }
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
		bool canEat = false;
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
									Move move = new Move(piece, piece.cell.bottomLeft.bottomLeft, piece.cell.bottomLeft.piece);
									validMoves.Add(move);
									canEat = true;
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
									Move move = new Move(piece, piece.cell.bottomRight.bottomRight, piece.cell.bottomRight.piece);
									validMoves.Add(move);
									canEat = true;
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
									Move move = new Move(piece, piece.cell.topLeft.topLeft, piece.cell.topLeft.piece);
									validMoves.Add(move);
									canEat = true;
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
									Move move = new Move(piece, piece.cell.topRight.topRight, piece.cell.topRight.piece);
									validMoves.Add(move);
									canEat = true;
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
					if (piece.cell.topLeft != null)
					{
						if (!(piece.cell.topLeft.piece == null))
						{
							if (piece.cell.topLeft.topLeft != null && piece.cell.topLeft.piece.type != piece.type)
							{
								//Occupied so check if resultant cell is empty so can eat
								if (piece.cell.topLeft.topLeft.piece == null)
								{
									Move move = new Move(piece, piece.cell.topLeft.topLeft, piece.cell.topLeft.piece);
									validMoves.Add(move);
									canEat = true;
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
									Move move = new Move(piece, piece.cell.topRight.topRight, piece.cell.topRight.piece);
									validMoves.Add(move);
									canEat = true;
								}
							}
						}
						else
						{
							Move move2 = new Move(piece, piece.cell.topRight);
							validMoves.Add(move2);
						}
					}
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
									Move move = new Move(piece, piece.cell.bottomLeft.bottomLeft, piece.cell.bottomLeft.piece);
									validMoves.Add(move);
									canEat = true;
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
									Move move = new Move(piece, piece.cell.bottomRight.bottomRight, piece.cell.bottomRight.piece);
									validMoves.Add(move);
									canEat = true;
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
			}
		}
		//Remove all other moves if can eat
		if (canEat)
		{
			validMoves.RemoveAll(m => m.getJumped() == null);
		}
		return validMoves;
	}

	//Make the move
	public void makeMove()
	{
		bool ate = false;
		bool moved = false;

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
		Piece curPiece = selectedPiece;
		foreach (Move move in validMoves)
		{
			if (move.getCell() == selectedCell && move.getPiece() == selectedPiece)
			{
				selectedPiece.movePiece(selectedCell);
				moved = true;
				//Remove jumped over piece
				if (move.getJumped() != null)
				{
					move.getJumped().remove();
					ate = true;
				}
			}
		}

		if (ate)
		{
			if (turn == Turn.black)
			{
				Debug.Log("hit0");
				validMoves = getAllValidMoves(Piece.Type.black);
			}
			else if (turn == Turn.white)
			{
				Debug.Log("hit1");
				validMoves = getAllValidMoves(Piece.Type.white);
			}
			Move newMove = new Move(curPiece, null);
			foreach (Move move in validMoves)
			{
				if (newMove.getPiece() == move.getPiece() && move.getJumped() != null)
				{
					Debug.Log("hit3 " + move.getJumped().cell.row + "," + move.getJumped().cell.col);
					selectedCell = null;
					return;
				}
			}
		}

		if (moved)
		{
			selectedCell = null;
			turn = turn == Turn.black ? Turn.white : Turn.black;
			turnText.text = turn == Turn.black ? "Turn: Black" : "Turn: White";
			checkWin();
		}
		else
		{
			Debug.Log("Not in the list of valid moves");
			return;
		}
		//TODO: Make king if cell to move to is end row
	}

	//Check if pieces are left
	void checkWin()
	{
		Piece[] pieces = FindObjectsOfType<Piece>();
		bool blackExists = false;
		bool whiteExists = false;
		//Check black 
		foreach (Piece piece in pieces)
		{
			if (piece.isActive && piece.type == Piece.Type.black)
			{
				blackExists = true;
			}
		}
		//Check white
		foreach (Piece piece in pieces)
		{
			if (piece.isActive && piece.type == Piece.Type.white)
			{
				whiteExists = true;
			}
		}
		if (!whiteExists)
		{
			Debug.Log("Black wins");
		}
		if (!blackExists)
		{
			Debug.Log("White wins");
		}
	}

	void gameOver()
	{

	}
}
